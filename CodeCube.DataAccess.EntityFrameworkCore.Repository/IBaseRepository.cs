using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeCube.DataAccess.EntityFrameworkCore.Repository
{
    internal interface IBaseRepository<TEntity>
    {
        IQueryable<TEntity> Find();
        Task<TEntity> Find(Guid id);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> Get(Guid id);
        Task Add(TEntity entity);
        void Update(TEntity entity);
        Task Delete(Guid id);
        void Delete(TEntity entity);

        int Save();
        Task<int> SaveAsync();
    }
}
