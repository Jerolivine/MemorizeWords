using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Persistance.Context.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;
using MemorizeWords.Infrastructure.Persistance.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistance.Repository
{
    public class UserHubRepository : EFCoreRepository<UserWordHubEntity, int>, IUserHubRepository, IBusinessRepository
    {
        public UserHubRepository(EFCoreDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<int?> GetLatestWordAnswerId()
        {
            return await Queryable().OrderByDescending(x => x.WordAnswerId).Select(x => x.WordAnswerId).FirstOrDefaultAsync();
        }

        public async Task UpdateUserHubAsync(int wordAnswerId)
        {
            var userHub = await Queryable().FirstOrDefaultAsync();
            if (userHub is null)
            {
                await AddAsnyc(new()
                {
                    WordAnswerId = wordAnswerId
                });
            }
            else
            {
                userHub.WordAnswerId = wordAnswerId;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
