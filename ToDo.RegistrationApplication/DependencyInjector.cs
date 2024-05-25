using Autofac;

namespace ToDo.RegistrationApplication
{
    public static class DependencyInjector
    {
        public static void Load(ContainerBuilder serviceCollection)
        {
            Services.DependencyInjector.Load(serviceCollection);
            DataAccess.DependencyInjector.Load(serviceCollection);
            ToDo.Utilities.DependencyInjector.Load(serviceCollection);
        }
    }
}
