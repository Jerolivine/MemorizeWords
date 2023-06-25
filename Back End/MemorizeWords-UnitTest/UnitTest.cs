using MemorizeWords.Context.EFCore;
using MemorizeWords.Entity;
using MemorizeWords.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemorizeWords_UnitTest
{
    public class UnitTest
    {
        private readonly IConfiguration _configuration;

        public UnitTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .Build();
        }

        [Fact]
        public void AddWord()
        {
            string word = "Word";
            string meaning = "Meaning";

            var options = new DbContextOptionsBuilder<MemorizeWordsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new MemorizeWordsDbContext(options,_configuration))
            {
                var entity = new WordEntity() { Word = word, Meaning = meaning };
                context.Add(entity);

                context.SaveChanges();

                var addedObject = context.Word.FirstOrDefault(x => x.Word == word && x.Meaning == meaning);

                Assert.True(addedObject != null, "Word didnt added.");
            }
        }

    }
}