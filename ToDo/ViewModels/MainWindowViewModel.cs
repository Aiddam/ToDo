using Avalonia.Controls;
using ToDo.Models.Models;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public UserControl DefaultView { get; set; } = new UserControl();

        private User? _currentUser;
        public override User? CurrentUser 
        { 
            get => _currentUser;
            set => _currentUser = value;
        }
    }
}
