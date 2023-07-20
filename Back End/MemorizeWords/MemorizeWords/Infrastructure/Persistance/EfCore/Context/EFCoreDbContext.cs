using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistance.FCore.Context
{
    public class EFCoreDbContext : DbContext
    {
        public DbSet<WordEntity> Word => Set<WordEntity>();
        public DbSet<WordAnswerEntity> WordAnswer => Set<WordAnswerEntity>();
        public DbSet<UserHubEntity> UserHub => Set<UserHubEntity>();

        public IConfiguration Configuration { get; }

        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options, IConfiguration configuration)
           : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Configuration.GetConnectionString("MemorizeWordsDb");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFCoreDbContext).Assembly);
        }
    }
}
