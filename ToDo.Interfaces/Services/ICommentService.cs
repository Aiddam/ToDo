using ToDo.Models.Models;

namespace ToDo.Interfaces.Services
{
    public interface ICommentService
    {
        Task CreateAsync(Comment comment, CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>?> GetCommentsAsync(int taskItemId, CancellationToken cancellationToken = default);
        Task DeleteAsync(int commentId);
    }
}
