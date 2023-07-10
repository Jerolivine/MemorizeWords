using MemorizeWords.Infrastructure.Entity.Core.Interfaces.Repository;
using MemorizeWords.Infrastructure.Persistance.Context.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;

namespace MemorizeWords.Infrastructure.Configuration.DbConfiguration.EfCore
{
    public static class EfCoreConfiguration
    {
        public static void ConfigureEfCore(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EFCoreDbContext>();
            builder.Services.AddTransient(typeof(IRepository<,>), typeof(EFCoreRepository<,>));
        }
    }
}
