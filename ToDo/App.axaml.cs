using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ToDo.ViewLocators;
using ToDo.ViewModels;
using ToDo.Views;
using Microsoft.Extensions.Configuration;

namespace ToDo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceContainer = GetServiceContainer();

        var mainViewModel = serviceContainer.Resolve<MainViewModel>();
        mainViewModel.ViewModel = serviceContainer.Resolve<LoginOrRegisterViewModel>();
        var mainView = new MainView()
        {
            DataContext = mainViewModel
        };

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
                {
                    DefaultView = mainView
                }
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = mainView;
        }

        DataTemplates.Add(serviceContainer.Resolve<ViewLocator>());

        base.OnFrameworkInitializationCompleted();
    }

    private static IContainer GetServiceContainer()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder
            .RegisterInstance(GetConfiguration())
            .AsImplementedInterfaces();

        DependencyInjector.Load(containerBuilder);

        return containerBuilder.Build();
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .Build();
    }
}
