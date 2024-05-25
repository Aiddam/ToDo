using ToDo.Interfaces.DataAccessInterfaces;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;

namespace ToDo.Services.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();

            await taskItemRepository.CreateAsync(taskItem, cancellationToken);

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int taskItemId)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();
            await taskItemRepository.DeleteAsync(taskItemId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<TaskItem> GetTaskItemAsync(int taskItemId, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();
            return await taskItemRepository.ReadEntityByIdAsync(taskItemId, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>?> GetTaskItemsAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();

            var taskItems = await taskItemRepository.ReadEntitiesByPredicate(e => e.TenantId == tenantId, cancellationToken: cancellationToken);

            return taskItems;
        }

        public async Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();
            return await taskItemRepository.UpdateAsync(taskItem, cancellationToken);
        }
    }
}
