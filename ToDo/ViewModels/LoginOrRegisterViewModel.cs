using Autofac;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;

namespace ToDo.ViewModels
{
    public class LoginOrRegisterViewModel : ViewModelBase
    {
        private readonly IComponentContext _componentContext;
        public ViewModelBaseWithParameters<Func<User, Task>>? LoginViewModel { get; set; }
        public ViewModelBaseWithParameters<Func<User, Task>>? RegisterViewModel { get; set; }

        private User? _currentUser = null;

        public LoginOrRegisterViewModel(IComponentContext componentContext)
        {
            _componentContext = componentContext;
            LoginViewModel = _componentContext.Resolve<LoginViewModel>(); 
            LoginViewModel.SetAdditionalParameter(ChangeViewToHomeView);
            RegisterViewModel = _componentContext.Resolve<RegistrationViewModel>();
            RegisterViewModel.SetAdditionalParameter(ChangeViewToHomeView);
        }

        public override User? CurrentUser 
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public async Task ChangeViewToHomeView(User user)
        {
            var homeViewModel = _componentContext.Resolve<HomeViewModel>();
            await homeViewModel.SetAdditionalParameter(user);
            ChangeView(homeViewModel);
            await homeViewModel.InitializeTabs();
        }

    }
}
