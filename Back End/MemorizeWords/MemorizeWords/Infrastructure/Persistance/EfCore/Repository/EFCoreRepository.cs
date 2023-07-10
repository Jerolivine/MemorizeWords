using MemorizeWords.Infrastructure.Entity.Core.Interfaces;
using MemorizeWords.Infrastructure.Entity.Core.Interfaces.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MemorizeWords.Infrastructure.Persistance.Context.Repository
{
    public class EFCoreRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new() where TPrimaryKey : struct
    {
        // TODO: Add UnitOfWork
        public EFCoreDbContext _dbContext { get; set; }
        public EFCoreRepository(EFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TPrimaryKey Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity.Id;
        }

        public Task<TPrimaryKey> AddAsnyc(TEntity entity)
        {
            return Task.Run(() => { return Add(entity); });
        }

        public void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbContext.Attach(entity);
            }
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() => { Delete(entity); });
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _dbContext.Set<TEntity>().SingleOrDefault(filter);
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.Run(() => { return Get(filter); });
        }

        public TEntity GetById(TPrimaryKey id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return Task.Run(() => { return GetById(id); });
        }
        public List<TEntity> GetList()
        {
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>();
            return queryable.ToList();
        }

        public Task<List<TEntity>> GetListAsync()
        {
            return Task.Run(() => { return GetList(); });
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbContext.Set<TEntity>();
        }

        public TEntity Update(TEntity entity)
        {
            TEntity entityDb = GetById(entity.Id);
            _dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public Task<TEntity> UpdateAsnyc(TEntity entity)
        {
            return Task.Run(() => { return Update(entity); });
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

    }
}
