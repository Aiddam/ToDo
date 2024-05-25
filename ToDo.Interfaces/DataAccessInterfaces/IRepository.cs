using ToDo.Models.Models;
using System.Linq.Expressions;

namespace ToDo.Interfaces.DataAccessInterfaces
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> ReadEntityByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> ReadEntitiesByPredicate(Expression<Func<T, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<T, object>>, bool>>? orderBy = null, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
