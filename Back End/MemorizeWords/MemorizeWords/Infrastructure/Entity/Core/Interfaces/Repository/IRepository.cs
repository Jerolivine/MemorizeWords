using MemorizeWords.Infrastructure.Entity.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MemorizeWords.Infrastructure.Entity.Core.Interfaces.Repository
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : IEntity<TPrimaryKey>, new() where TPrimaryKey : struct
    {
        TPrimaryKey Add(TEntity entity);
        Task<TPrimaryKey> AddAsnyc(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);

        TEntity Get(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);

        TEntity GetById(TPrimaryKey id);
        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsnyc(TEntity entity);

        //IQueryable<TEntity> Find(bool getTrash, int pageNo, int pageSize, Expression<Func<TEntity, bool>> predicate = null);
        //IQueryable<TEntity> GetPaging(bool getTrash, int pageNo, int pageSize);

        List<TEntity> GetList();
        Task<List<TEntity>> GetListAsync();

        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
