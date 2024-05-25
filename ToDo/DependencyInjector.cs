using Autofac;

namespace ToDo
{
    public static class DependencyInjector
    {
        public static void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(typeof(DependencyInjector).Assembly)
                .Where(t => 
                    t.Name.Contains("ViewModel") ||
                    t.Name.Contains("ViewLocator")
                )
                .AsSelf()
                .AsImplementedInterfaces();

            Services.DependencyInjector.Load(containerBuilder);
            Utilities.DependencyInjector.Load(containerBuilder);
        }
    }
}
