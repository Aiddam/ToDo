using ToDo.Interfaces.Services;
using ToDo.Models.Enum;
using ToDo.Models.Models;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDo.ViewModels
{
    public class RegistrationViewModel : ViewModelBaseWithParameters<Func<User, Task>>
    {
        private readonly IUserService _userService;
        private Func<User, Task> _changeView;

        private User? _user = null;

        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _role = string.Empty;
        private int _tenantId;
        private string _message = string.Empty;

        public RegistrationViewModel(IUserService userService)
        {
            _userService = userService;
            RegistrationCommand = ReactiveCommand.CreateFromTask(RegistrationCommandHandler);
        }

        public event Func<Task>? UserCreated;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }
        public int TenantId
        {
            get => _tenantId;
            set => this.RaiseAndSetIfChanged(ref _tenantId, value);
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
        public string Name
        {
            get => _name;
            set
            {
                this.RaiseAndSetIfChanged(ref _name, value);
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                this.RaiseAndSetIfChanged(ref _surname, value);
            }
        }
        public string Role
        {
            get => _role;
            set
            {
                this.RaiseAndSetIfChanged(ref _role, value);
            }
        }
        public ICommand RegistrationCommand { get; }
        public override User? CurrentUser
        {
            get => _user;
            set => _user = value;
        }

        public async Task RegistrationCommandHandler()
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname))
            {
                Message = "Поля не можуть бути пустими";
                return;
            }

            try
            {
                UserDto userDto = new() { Login = Login, Name = Name, Role = UserRole.Employee, Password = Password, Surname = Surname, TenantId = TenantId};
                var user = await _userService.RegistrationAsync(userDto);
                _ = _changeView(user);
            }
            catch (Exception ex)
            {
                Message = $"Не вийшло зареєструватися: {ex}";
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
