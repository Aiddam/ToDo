using ToDo.Interfaces.DataAccessInterfaces;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;

namespace ToDo.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            var commentRepository = await _unitOfWork.GetRepository<Comment>();

            await commentRepository.CreateAsync(comment, cancellationToken);

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int commentId)
        {
            var commentRepository = await _unitOfWork.GetRepository<Comment>();
            await commentRepository.DeleteAsync(commentId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Comment>?> GetCommentsAsync(int taskItemId, CancellationToken cancellationToken = default)
        {
            var commentRepository = await _unitOfWork.GetRepository<Comment>();

            var taskItems = await commentRepository.ReadEntitiesByPredicate(e => e.TaskItemId == taskItemId, cancellationToken: cancellationToken);

            return taskItems;
        }

    }
}
