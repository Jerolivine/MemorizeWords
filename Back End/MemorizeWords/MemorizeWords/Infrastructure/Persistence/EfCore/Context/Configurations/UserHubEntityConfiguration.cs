using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemorizeWords.Infrastructure.Persistence.EfCore.Context.Configurations
{
    public class UserHubEntityConfiguration : IEntityTypeConfiguration<UserHubEntity>
    {
        public void Configure(EntityTypeBuilder<UserHubEntity> builder)
        {
            builder.ToTable("USER_HUB");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.WordAnswerId).HasColumnName("WORD_ANSWER_ID").IsRequired();

        }
    }
}
