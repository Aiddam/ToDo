using Autofac;
using ToDo.Models.Models;
using ReactiveUI;
using System.Windows.Input;

namespace ToDo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IComponentContext _componentContext;

        private ViewModelBase _viewModel = null!;
        public ViewModelBase ViewModel
        {
            get => _viewModel;
            set
            {
                this.RaiseAndSetIfChanged(ref _viewModel, value);
                ViewModel.OnChangeViewModel += InternalChangeView;
            }
        }

        public ICommand SetActiveMenuItemCommand { get; }
        public override User? CurrentUser 
        {
            get => ViewModel.CurrentUser; 
            set => ViewModel.CurrentUser = value;
        }
        public bool IsLoggedIn => CurrentUser is not null;
        public ICommand LogoutButton { get; set; }

        public MainViewModel(IComponentContext componentContext)
        {
            _componentContext = componentContext;

            LogoutButton = ReactiveCommand.Create(() => InternalChangeView(_componentContext.Resolve<LoginOrRegisterViewModel>()));
        }

        private void InternalChangeView(ViewModelBase viewModel)
        {
            ViewModel = viewModel;

            this.RaisePropertyChanging(nameof(ViewModel));
            this.RaisePropertyChanged(nameof(CurrentUser));
            this.RaisePropertyChanged(nameof(IsLoggedIn));
        }
    }
}
