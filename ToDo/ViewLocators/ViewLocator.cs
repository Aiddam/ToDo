using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ToDo.ViewModels;
using System;
using System.Linq;

namespace ToDo.ViewLocators
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? param)
        {
            if (param is null) return null;

            var viewName = param.GetType().Name.Replace("ViewModel", "View");
            var viewType = typeof(ViewLocator).Assembly.GetTypes().FirstOrDefault(t => t.Name == viewName);
            if (viewType is null) return null;

            var view = Activator.CreateInstance(viewType) as UserControl;
            if (view is null) return null;
            view.DataContext = param;

            return view;
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
