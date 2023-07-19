using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Persistance.Context.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;
using MemorizeWords.Infrastructure.Persistance.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistance.Repository
{
    public class UserHubRepository : EFCoreRepository<UserHubEntity, int>, IUserHubRepository, IBusinessRepository
    {
        public UserHubRepository(EFCoreDbContext dbContext) : base(dbContext)
        {
        }

        public async Task UpdateUserHubAsnyc(int wordAnswerId)
        {
            UserHubEntity entity = await GetUserHubAsync();
            if (entity is null)
            {
                await AddAsnyc(new UserHubEntity()
                {
                    WordAnswerId = wordAnswerId
                });
            }
            else
            {
                entity.WordAnswerId = wordAnswerId;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<UserHubEntity> GetUserHubAsync()
        {
            return await Queryable().FirstOrDefaultAsync();
        }

    }
}
