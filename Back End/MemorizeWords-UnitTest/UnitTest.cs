using MemorizeWords.Context.EFCore;
using MemorizeWords.Entity;
using MemorizeWords.Models;
using MemorizeWords.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemorizeWords_UnitTest
{
    public class UnitTest
    {
        private readonly IConfiguration _configuration;
        private readonly string WORD = "WORD";
        private readonly string MEANING = "MEANING";

        public UnitTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
        }

        [Fact]
        public void AddWord_ShouldAddWord()
        {

            DbContextOptions<MemorizeWordsDbContext> options = GetDbContextOptions();

            using (var memorizeWordsDbContext = new MemorizeWordsDbContext(options, _configuration))
            {
                WordEntity? wordEntity = AddWord(memorizeWordsDbContext);

                Assert.True(wordEntity != null, "Word didnt added.");
            }
        }

        private WordEntity AddWord(MemorizeWordsDbContext memorizeWordsDbContext)
        {
            var wordEntity = memorizeWordsDbContext.Word.FirstOrDefault(x => x.Word.ToUpper().Equals(WORD.ToUpper()));
            if (wordEntity != null)
            {
                wordEntity.Meaning = MEANING;
            }
            else
            {
                wordEntity = new WordEntity() { Word = WORD, Meaning = MEANING };
                memorizeWordsDbContext.Add(wordEntity);
            }

            var addedWordEntity = memorizeWordsDbContext.Word.FirstOrDefault();

            return addedWordEntity;
        }

        [Fact]
        public void Answer_ShouldAddRecord()
        {
            DbContextOptions<MemorizeWordsDbContext> options = GetDbContextOptions();

            using (var memorizeWordsDbContext = new MemorizeWordsDbContext(options, _configuration))
            {
                WordEntity? wordEntity = AddWord(memorizeWordsDbContext);

                bool answer = wordEntity.Meaning.ToUpper().Equals(MEANING.ToUpper());

                memorizeWordsDbContext.WordAnswer.Add(new WordAnswerEntity()
                {
                    WordId = wordEntity.Id,
                    Answer = answer,
                    AnswerDate = DateTime.Now,
                });

                memorizeWordsDbContext.SaveChanges();

                var answerEntity = memorizeWordsDbContext.WordAnswer.FirstOrDefault();
                Assert.True(answerEntity != null, "WordAnswer didnt added.");
            }
        }

        private static DbContextOptions<MemorizeWordsDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<MemorizeWordsDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;
        }
    }
}