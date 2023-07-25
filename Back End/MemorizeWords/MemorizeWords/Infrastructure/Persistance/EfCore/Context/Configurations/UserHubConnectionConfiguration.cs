using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemorizeWords.Infrastructure.Persistance.FCore.Context.Configurations
{
    public class UserHubConnectionConfiguration : IEntityTypeConfiguration<UserHubConnectionEntity>
    {
        public void Configure(EntityTypeBuilder<UserHubConnectionEntity> builder)
        {
            builder.ToTable("USER_HUB_CONNECTION");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.UserId).HasColumnName("USER_ID").IsRequired();
            builder.Property(x => x.HubContext).HasColumnName("HUB_CONTEXT").IsRequired();

        }
    }
}
