using Autofac;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDo.ViewModels
{
    public class LoginViewModel : ViewModelBaseWithParameters<Func<User, Task>>
    {
        private readonly IUserService _userService;
        private readonly IComponentContext _componentContext;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private Func<User, Task> _changeView;

        private User? _currentUser = null;

        public LoginViewModel(IUserService userService, IComponentContext componentContext)
        {
            _userService = userService;
            _componentContext = componentContext;
            LoginCommand = ReactiveCommand.CreateFromTask(LoginCommandHandler);
        }

        public string Password
        {
            get => _password;
            set
            {
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                this.RaiseAndSetIfChanged(ref _login, value);
            }
        }

        public ICommand LoginCommand { get; }

        public override User? CurrentUser 
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public async Task LoginCommandHandler()
        {
            if (string.IsNullOrEmpty(Login)) return;
            if (string.IsNullOrEmpty(Password)) return;

            try
            {
                var user = await _userService.LoginAsync(Login, Password);
                _changeView(user);
            }
            catch
            {
                return;
            }
        }

        public override Task SetAdditionalParameter(Func<User, Task> parameter)
        {
            _changeView = parameter;
            return Task.CompletedTask;
        }
    }
}
