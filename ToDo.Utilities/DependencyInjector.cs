using Autofac;
using AutoMapper;

namespace ToDo.Utilities
{
    public static class DependencyInjector
    {
        public static void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(typeof(DependencyInjector).Assembly)
                .AsSelf()
                .AsImplementedInterfaces();

            containerBuilder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(DependencyInjector).Assembly);
                });

                return config.CreateMapper();
            }).As<IMapper>().SingleInstance();
        }
    }
}
