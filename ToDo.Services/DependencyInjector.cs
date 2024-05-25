using Autofac;

namespace ToDo.Services
{
    public static class DependencyInjector
    {
        public static void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(typeof(DependencyInjector).Assembly)
                .AsSelf()
                .AsImplementedInterfaces();

            DataAccess.DependencyInjector.Load(containerBuilder);
        }
    }
}
