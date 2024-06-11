using Avalonia.Controls;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class CreateTaskView : UserControl
    {
        public CreateTaskView()
        {
            InitializeComponent();
        }
        private async void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var viewModel = DataContext as CreateTaskViewModel;
            if (viewModel is null) return;
            viewModel.ClearData();
        }
    }
}
