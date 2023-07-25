using MemorizeWords.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemorizeWords.Infrastructure.Persistence.EfCore.Context.Configurations
{
    public class WordAnswerEntityConfiguration : IEntityTypeConfiguration<WordAnswerEntity>
    {
        public void Configure(EntityTypeBuilder<WordAnswerEntity> builder)
        {
            builder.ToTable("WORD_ANSWER");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.WordId).HasColumnName("WORD_ID").IsRequired();
            builder.Property(x => x.Answer).HasColumnName("ANSWER").IsRequired();
            builder.Property(x => x.AnswerDate).HasColumnName("ANSWER_DATE").IsRequired();
            builder.Property(x => x.UserId).HasColumnName("USER_ID");
        }
    }
}
