using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MemorizeWords.Context.EFCore
{
    public class MemorizeWordsDbContext : DbContext
    {
        public DbSet<WordEntity> Word => Set<WordEntity>();
        public DbSet<WordAnswerEntity> WordAnswer => Set<WordAnswerEntity>();

        public IConfiguration Configuration { get; }

        public MemorizeWordsDbContext(DbContextOptions<MemorizeWordsDbContext> options, IConfiguration configuration)
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemorizeWordsDbContext).Assembly);
        }
    }
}
