using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Persistence.EfCore.Context;
using MemorizeWords.Infrastructure.Persistence.EfCore.Repository;
using MemorizeWords.Infrastructure.Persistence.Interfaces;
using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistence.Repository
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
