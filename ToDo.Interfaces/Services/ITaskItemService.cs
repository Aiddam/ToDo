using ToDo.Models.Models;

namespace ToDo.Interfaces.Services
{
    public interface ITaskItemService
    {
        Task CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task<IEnumerable<TaskItem>?> GetTaskItemsAsync(int tenantId, CancellationToken cancellationToken = default);
        Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task DeleteAsync(int taskItemId);
        Task<TaskItem> GetTaskItemAsync(int taskItemId, CancellationToken cancellationToken = default);
        Task GenerateReportAsync(int tenantId, CancellationToken cancellationToken = default);
    }
}
