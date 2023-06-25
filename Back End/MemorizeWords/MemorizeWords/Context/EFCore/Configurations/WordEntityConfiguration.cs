using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MemorizeWords.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizeWords.Context.EFCore.Configurations
{
    public class WordEntityConfiguration : IEntityTypeConfiguration<WordEntity>
    {
        public void Configure(EntityTypeBuilder<WordEntity> builder)
        {
            builder.ToTable("WORD");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Word).HasColumnName("WORD").IsRequired();
            builder.Property(x =>x.Meaning).HasColumnName("MEANING").IsRequired();

            builder.HasMany(a => a.WordAnswers)
               .WithOne(b => b.Word)
               .HasForeignKey(b => b.WordId);
            }
    }
}
