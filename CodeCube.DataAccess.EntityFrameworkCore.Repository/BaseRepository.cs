using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCube.DataAccess.EntityFrameworkCore.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected ApplicationDbContext Context;

        private DbSet<TEntity> Entities;

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            Context = applicationDbContext;
            Entities = Context.Set<TEntity>();
        }

        public IQueryable<TEntity> Find()
        {
            return Entities.AsNoTracking();
        }

        public async Task<TEntity> Find(Guid id)
        {
            return await Entities.AsNoTracking().SingleOrDefaultAsync(o => o.Id == id);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.AsNoTracking().Where(expression);
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await Entities.SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task Add(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public async Task Delete(Guid id)
        {
            TEntity entityToDelete = await Entities.SingleOrDefaultAsync(o => o.Id == id);
            if (entityToDelete == null) return;

            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
