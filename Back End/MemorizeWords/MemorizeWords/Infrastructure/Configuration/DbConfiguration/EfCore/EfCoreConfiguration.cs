using MemorizeWords.Infrastructure.Entity.Core.Interfaces.Repository;
using MemorizeWords.Infrastructure.Persistence.EfCore.Context;
using MemorizeWords.Infrastructure.Persistence.EfCore.Repository;

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
