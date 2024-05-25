using Autofac;
using ToDo.Models.Enum;
using ToDo.Models.Models;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDo.ViewModels;

public class HomeViewModel : ViewModelBaseWithParameters<User>
{
    public ViewModelBaseWithParameters<User>? CreateTaskViewModel { get; set; }

    private User? _user = null!;
    private readonly IComponentContext _context;
    public ViewModelBaseWithParameters<User>? ToDoViewModel { get; set; }
    public ICommand LogoutButton { get; set; }

    public bool IsRegistrtionShow
    {
        get => _user!.Role == UserRole.Owner;
    }

    public override User? CurrentUser 
    {
        get => _user; 
        set => _user = value; 
    }

    public HomeViewModel(IComponentContext context)
    {
        _context = context;
      ;
        LogoutButton = ReactiveCommand.Create(() => ChangeView(_context.Resolve<LoginOrRegisterViewModel>()));
    }
    public override Task SetAdditionalParameter(User parameter)
    {
        _user = parameter;
        return Task.CompletedTask;
    }
    public async Task InitializeTabs()
    {
        var toDoViewModel = _context.Resolve<ToDoViewModel>();
        ToDoViewModel = toDoViewModel;
        await ToDoViewModel.SetAdditionalParameter(_user);
        foreach (var dlgt in GetDelegate())
        {
            ToDoViewModel.OnChangeViewModel += (Action<ViewModelBase>)dlgt;
        }

        var createTaskViewModel = _context.Resolve<CreateTaskViewModel>();
        CreateTaskViewModel = createTaskViewModel;
        await CreateTaskViewModel.SetAdditionalParameter(_user);
        foreach (var dlgt in GetDelegate())
        {
            CreateTaskViewModel.OnChangeViewModel += (Action<ViewModelBase>)dlgt;
        }
    }
}
