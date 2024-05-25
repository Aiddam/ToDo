using Autofac;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;
using ToDo.ViewModelParameters;

namespace ToDo.ViewModels;  

public class EditTaskViewModel : ViewModelBaseWithParameters<EditTaskModelParameter>
{
    private readonly ITaskItemService _taskItemService;
    private readonly ICommentService _commentService;
    private readonly IComponentContext _componentContext;
    public EditTaskViewModel(ITaskItemService taskItemService, ICommentService commentService, IComponentContext componentContext)
    {
        _taskItemService = taskItemService;
        _componentContext = componentContext;
        _commentService = commentService;
        EditTaskItemCommand = ReactiveCommand.CreateFromTask(EditTaskItemCommandHandler);
        GoBackCommand = ReactiveCommand.CreateFromTask(GoBackCommandHandler);
    }
    private User? _user = null;
    private TaskItem? _taskItem = null;
    private ObservableCollection<Comment>? _comments;
    private int _taskItemId;
    private string _comment;

    public string Comment
    {
        get => _comment;
        set => this.RaiseAndSetIfChanged(ref _comment, value);
    }
    public override User? CurrentUser
    {
        get => _user;
        set => _user = value;
    }
    public TaskItem? TaskItem
    {
        get => _taskItem;
        set => this.RaiseAndSetIfChanged(ref _taskItem, value);
    }
    public ObservableCollection<Comment>? Comments
    {
        get => _comments;
        set => this.RaiseAndSetIfChanged(ref _comments, value);
    }
    public override Task SetAdditionalParameter(EditTaskModelParameter parameter)
    {
        _user = parameter.User;
        _taskItemId = parameter.TaskItemId;
        return Task.CompletedTask;
    }
    public ICommand EditTaskItemCommand { get; }
    public ICommand GoBackCommand { get; }
    public async Task EditTaskItemCommandHandler()
    {
        try
        {
            await _taskItemService.UpdateTaskItemAsync(TaskItem);
            await GoBackCommandHandler();
        }
        catch (Exception)
        {
            return;
        }
    }
    public async Task LoadTaskItemAsync()
    {
        TaskItem = await _taskItemService.GetTaskItemAsync(_taskItemId);
        Comments =  new ObservableCollection<Comment>(TaskItem.Comments);
    }
    public async Task CreateCommentAsync()
    {
        var comment = new Comment() { TaskItemId = _taskItemId, UserId = CurrentUser.Id, Content = Comment };
        await _commentService.CreateAsync(comment);
        comment.User = CurrentUser;
        Comments.Add(comment);
        Comment = string.Empty;
    }
    public async Task GoBackCommandHandler()
    {
        var homeViewModel = _componentContext.Resolve<HomeViewModel>();
        await homeViewModel.SetAdditionalParameter(_user!);
        ChangeView(homeViewModel);
        await homeViewModel.InitializeTabs();
    }
}
