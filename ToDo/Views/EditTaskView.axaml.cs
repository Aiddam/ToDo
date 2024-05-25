using System;
using Avalonia.Controls;
using Avalonia.Input;
using ToDo.Models.Models;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class EditTaskView : UserControl
    {
        public EditTaskView()
        {
            InitializeComponent();
        }
        private async void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is not EditTaskViewModel viewModel) return;
            await viewModel.LoadTaskItemAsync();
        }
        private async void CommentTextBoxEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            {
                if (DataContext is EditTaskViewModel viewModel) await viewModel.CreateCommentAsync();
            }
        }

    }
}
