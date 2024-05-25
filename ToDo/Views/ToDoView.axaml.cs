using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Specialized;
using System.Drawing;
using ToDo.Models.Models;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class ToDoView : UserControl
    {
        private Point _ghostPosition = new(0, 0);
        private readonly Point _mouseOffset = new(-5, -5);
        public ToDoView()
        {
            InitializeComponent();
            AddHandler(DragDrop.DragOverEvent, DragOver);
            AddHandler(DragDrop.DropEvent, Drop);
        }
        private async void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetCurrentPoint(sender as Control);
            if (point.Properties.IsRightButtonPressed)
            {
                return;
            }
            if (sender is not Border border) return;
            if (border.DataContext is not TaskItem taskItem) return;

            var ghostPos = GhostItem.Bounds.Position;
            _ghostPosition = new Point((int)ghostPos.X  + _mouseOffset.X, (int)ghostPos.Y + _mouseOffset.Y);

            var mousePos = e.GetPosition(MainPanel);
            var offsetX = mousePos.X - ghostPos.X;
            var offsetY = mousePos.Y - ghostPos.Y + _mouseOffset.X;
            GhostItem.RenderTransform = new TranslateTransform(offsetX, offsetY);

            if (DataContext is not ToDoViewModel vm) return;
            vm.StartDrag(taskItem);
            
            GhostItem.IsVisible = true;

            var dragData = new DataObject();
            dragData.Set(ToDoViewModel.CustomFormat, taskItem);
            await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
            GhostItem.IsVisible = false;
           
        }
        private async void DeleteItem(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not ToDoViewModel vm) return;
            if (e.Source is Control control && control.DataContext is TaskItem item)
            {
                await vm.DeleteAsync(item);
            }
        }
        private void EditItem(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not ToDoViewModel vm) return;
            if (e.Source is Control control && control.DataContext is TaskItem item)
            {
                vm.GoToEditTaskItem(item.Id);
            }
        }
        private void DragOver(object? sender, DragEventArgs e)
        {
            var currentPosition = e.GetPosition(MainPanel);

            var offsetX = currentPosition.X - _ghostPosition.X;
            var offsetY = currentPosition.Y - _ghostPosition.Y;

            GhostItem.RenderTransform = new TranslateTransform(offsetX, offsetY);

            // set drag cursor icon
            e.DragEffects = DragDropEffects.Move;
            if (DataContext is not ToDoViewModel vm) return;
            var data = e.Data.Get(ToDoViewModel.CustomFormat);
            if (data is not TaskItem taskItem) return;
        }

        private void Drop(object? sender, DragEventArgs e)
        {
            var data = e.Data.Get(ToDoViewModel.CustomFormat);

            if (data is not TaskItem taskItem)
            {
                Console.WriteLine("No task item");
                return;
            }

            if (DataContext is not ToDoViewModel vm) return;
            var source = e.Source as ScrollContentPresenter;

            if(source != null)
            {
                vm.Drop(taskItem, (source.Parent.Parent as Control)?.Name);
            }
        }
        private async void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            GhostItem.IsVisible = false;
            var viewModel = DataContext as ToDoViewModel;
            if (viewModel is null) return;
            viewModel.CollectionItems.CollectionChanged += OnCollectionChanged;
            await viewModel.LoadToDosAsync();
        }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Handle UI update logic here
            RefreshUI();
        }
        private void RefreshUI()
        {
            var currentDataContext = DataContext;
            DataContext = null;
            DataContext = currentDataContext;
        }

        private void MenuItem_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }
    }
}
