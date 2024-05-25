using ToDo.Models.Models;
using Microsoft.Data.SqlClient;

namespace ToDo.Interfaces.DataAccessInterfaces
{
    public interface IUnitOfWork
    {
        Task<IRepository<T>> GetRepository<T>()
            where T : BaseEntity;
        Task CommitAsync();
        Task<object?> ExecuteSqlQueryAsync(string sqlQuery, IEnumerable<SqlParameter> parameters, string fieldName);
    }
}
