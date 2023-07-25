using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemorizeWords.Infrastructure.Persistance.FCore.Context.Configurations
{
    public class UserWordHubEntityConfiguration : IEntityTypeConfiguration<UserWordHubEntity>
    {
        public void Configure(EntityTypeBuilder<UserWordHubEntity> builder)
        {
            builder.ToTable("USER_WORD_HUB");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.WordAnswerId).HasColumnName("WORD_ANSWER_ID").IsRequired();

        }
    }
}
