using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;

namespace ToDo.ViewModels;  

public class CreateTaskViewModel : ViewModelBaseWithParameters<User>
{
    private readonly ITaskItemService _taskItemService;
    public CreateTaskViewModel(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
        CreateTask = ReactiveCommand.CreateFromTask(CreateTaskCommandHandler);
    }
    private User? _user = null;
    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _message = string.Empty;
    public override User? CurrentUser
    {
        get => _user;
        set => _user = value;
    }
    public override Task SetAdditionalParameter(User parameter)
    {
        _user = parameter;
        return Task.CompletedTask;
    }
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    public string Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
    public ICommand CreateTask { get; }
    public void ClearData()
    {
        Message = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
    }
    public async Task CreateTaskCommandHandler()
    {
        Message = string.Empty;
        if (string.IsNullOrEmpty(Title))
        {
            Message = "Поле назва не можу бути пустою";
            return;
        }

        try
        {
            var taskItem = new TaskItem();
            taskItem.Title = Title;
            taskItem.Description = Description;
            taskItem.TenantId = CurrentUser.TenantId;
            await _taskItemService.CreateAsync(taskItem);
            Message = "Задача успішно створена";
        }
        catch (Exception ex)
        {
            Message = $"Не вийшло створити задачу: {ex}";
            return;
        }
    }
}