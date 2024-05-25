using ToDo.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ToDo.DataAccess.Repositories
{
    public class TaskItemRepository : BaseRepository<TaskItem>
    {
        public TaskItemRepository(DbContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<TaskItem>> ReadEntitiesByPredicate(Expression<Func<TaskItem, bool>> predicate, IEnumerable<KeyValuePair<Expression<Func<TaskItem, object>>, bool>>? orderBy = null,
    CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(e => e.Comments)
                .Where(predicate);

            if (orderBy is not null)
            {
                foreach (var order in orderBy)
                {
                    query = order.Value ? query.OrderBy(order.Key) : query.OrderByDescending(order.Key);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }
        public override async Task<TaskItem?> ReadEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.Comments)
                .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
    }
}
