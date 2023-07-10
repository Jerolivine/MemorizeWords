using MemorizeWords.Infrastructure.Application.Interfaces;

namespace MemorizeWords.Infrastructure.Configuration.ServiceInjector
{
    public static class ServiceInjector
    {
        public static void InjectServices(this WebApplicationBuilder builder)
        {
            InjectService<IBusinessService>(builder);
            InjectService<IBusinessRepository>(builder);
            
        }

        private static void InjectService<TInterface>(this WebApplicationBuilder builder)
        {
            builder.Services.Scan(scan => scan
                    .FromAssemblyOf<TInterface>()
                    .AddClasses(classes => classes.AssignableTo<TInterface>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }
    }
}
