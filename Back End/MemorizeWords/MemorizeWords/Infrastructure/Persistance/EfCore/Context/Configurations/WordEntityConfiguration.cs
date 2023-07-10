using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemorizeWords.Infrastructure.Persistance.FCore.Context.Configurations
{
    public class WordEntityConfiguration : IEntityTypeConfiguration<WordEntity>
    {
        public void Configure(EntityTypeBuilder<WordEntity> builder)
        {
            builder.ToTable("WORD");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Word).HasColumnName("WORD").IsRequired();
            builder.Property(x => x.Meaning).HasColumnName("MEANING").UseCollation("Turkish_CI_AI").IsRequired();
            builder.Property(x => x.IsLearned).HasColumnName("IS_LEARNED").IsRequired();
            builder.Property(x => x.WritingInLanguage).HasColumnName("WRITING_IN_LANGUAGE");

            builder.HasMany(a => a.WordAnswers)
               .WithOne(b => b.Word)
               .HasForeignKey(b => b.WordId);
        }
    }
}
