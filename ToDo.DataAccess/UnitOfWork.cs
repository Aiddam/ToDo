using ToDo.Interfaces.DataAccessInterfaces;
using ToDo.DataAccess.DbContexts;
using ToDo.DataAccess.Repositories;
using ToDo.Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ToDo.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainDbContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(MainDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public Task<IRepository<T>> GetRepository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (_repositories.ContainsKey(type)) return Task.FromResult((IRepository<T>)_repositories[type]);

            var repositoryType = typeof(UnitOfWork)
                                     .Assembly
                                     .GetTypes()
                                     .FirstOrDefault(t => t.IsAssignableTo(typeof(IRepository<T>)))
                                 ??
                                 typeof(BaseRepository<T>);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context)
                                     ?? throw new ArgumentException($"Unable to resolve repository with type {typeof(T).Name}");
            _repositories[type] = repositoryInstance;
            return Task.FromResult((IRepository<T>)_repositories[type]);
        }

        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
        public async Task<object?> ExecuteSqlQueryAsync(string sqlQuery, IEnumerable<SqlParameter> parameters, string fieldName)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sqlQuery;
            command.CommandType = CommandType.Text;
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }
            await _context.Database.OpenConnectionAsync();

            await using var result = await command.ExecuteReaderAsync();
            if (await result.ReadAsync())
            {
                return result[fieldName];
            }

            return null; // Return 0 if no data found or in case of exception
        }
    }
}
