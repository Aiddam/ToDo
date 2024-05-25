using Autofac;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Interfaces.Services;
using ToDo.Models;
using ToDo.Models.Enum;
using ToDo.Models.Models;
using ToDo.ViewModelParameters;

namespace ToDo.ViewModels;

public class ToDoViewModel : ViewModelBaseWithParameters<User>
{
    public const string CustomFormat = "task-item-format";
    public const string ToDo = "ToDo";
    public const string InProgress = "InProgress";
    public const string Done = "Done";

    private readonly ITaskItemService _taskItemService;
    private readonly IComponentContext _componentContext;
    private User? _user = null;

    private ObservableDictionary<string, IEnumerable<TaskItem>> _collectionItems = new ObservableDictionary<string, IEnumerable<TaskItem>>();
    private TaskItem? _draggingTaskItem;

    public ToDoViewModel(ITaskItemService taskItemService, IComponentContext componentContext)
    {
        _taskItemService = taskItemService;
        _componentContext = componentContext;
        DeleteCommand = ReactiveCommand.CreateFromTask(DeleteCommandHandler);
    }
    public ICommand DeleteCommand { get; }

    public override Task SetAdditionalParameter(User parameter)
    {
        _user = parameter;
        return Task.CompletedTask;
    }
    public override User? CurrentUser
    {
        get => _user;
        set => _user = value;
    }
    public ObservableDictionary<string, IEnumerable<TaskItem>> CollectionItems
    {
        get => _collectionItems;
    }
    public TaskItem? DraggingTaskItem
    {
        get => _draggingTaskItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _draggingTaskItem, value);
        }
    }
    public async Task DeleteCommandHandler()
    {
    }
    public void Drop(TaskItem taskItem, string statusName)
    {
        var key = taskItem.Status.ToString();
        var sourceList = CollectionItems[key].ToList();
        var item = sourceList.SingleOrDefault(t => t.Id == taskItem.Id);
        if (item is null)
        {
            return;
        }
        try
        {
            item.Status = Enum.Parse<Status>(statusName);
        }
        catch (Exception)
        {

            return;
        }
        sourceList.Remove(taskItem);
        var destinationList = CollectionItems[item.Status.ToString()].ToList();
        _taskItemService.UpdateTaskItemAsync(item);
        destinationList.ReplaceOrAdd(item, item);

        CollectionItems[key] = sourceList;
        CollectionItems[item.Status.ToString()] = destinationList;
    }
    public void StartDrag(TaskItem taskItem)
    {
        DraggingTaskItem = taskItem;
    }
    public async Task LoadToDosAsync()
    {
        CollectionItems.Clear();

        foreach (Status item in Enum.GetValues(typeof(Status)))
        {
            var taskItems = await _taskItemService.GetTaskItemsAsync(CurrentUser!.TenantId);
            CollectionItems.Add(item.ToString(), taskItems!.Where(tdi => tdi.Status == item).ToList());
        }
    }
    public async Task DeleteAsync(TaskItem taskItem)
    {
        var key = taskItem.Status.ToString();
        await _taskItemService.DeleteAsync(taskItem.Id);
        var sourceList = CollectionItems[key].ToList();
        CollectionItems[key] = CollectionItems[key].ToList().Where(ci=> ci.Id != taskItem.Id);
    }
    public void GoToEditTaskItem(int taskItemId)
    {
        var editTaskViewModel = _componentContext.Resolve<EditTaskViewModel>();
        editTaskViewModel.SetAdditionalParameter(new EditTaskModelParameter() { User = CurrentUser, TaskItemId = taskItemId});
        ChangeView(editTaskViewModel);
    }

}