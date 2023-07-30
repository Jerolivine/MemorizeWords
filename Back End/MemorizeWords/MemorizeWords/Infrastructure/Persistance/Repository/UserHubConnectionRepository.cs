using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Infrastructure.Persistence.EfCore.Context;
using MemorizeWords.Infrastructure.Persistence.EfCore.Repository;
using MemorizeWords.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistance.Repository
{
    public class UserHubConnectionRepository : EFCoreRepository<UserHubConnectionEntity, int>, IUserHubConnectionRepository, IBusinessRepository
    {
        public UserHubConnectionRepository(EFCoreDbContext dbContext) : base(dbContext)
        {
        }

        public async Task UpdateUserHubAsync(int userId, string hubContext)
        {
            var userHubEntity = await GetAsync(x => x.UserId == userId);
            if (userHubEntity == null)
            {
                await AddAsnyc(new()
                {
                    UserId = userId,
                    HubContext = hubContext
                });
            }
            else
            {
                userHubEntity.UserId = userId;
                userHubEntity.HubContext = hubContext;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserHubConnectionEntity>> GetUsersHub(List<int> userIds)
            => await Queryable().Where(x => userIds.Contains(x.UserId)).ToListAsync();

        public async Task<bool> IsAnyUserConnectedToHub()
            => await Queryable().AnyAsync();

        public async Task DeleteUser(int userId)
        {
            var userHubConnection = await Queryable().FirstOrDefaultAsync(x => x.UserId == userId);
            if (userHubConnection is not null)
            {
                await DeleteAsync(userHubConnection);
            }
        }

    }
}