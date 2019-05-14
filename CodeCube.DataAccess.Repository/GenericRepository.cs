//using CodeCube.DataAccess.EntityFramework.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Linq.Expressions;

//namespace CodeCube.DataAccess.EntityFramework
//{
//    //http://www.asp.net/mvc/tutorials/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
//    public abstract class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>, IDisposable
//        where TEntity : class, IBaseEntity
//        where TContext : class, IApplicationDataContext, new()

//    {
//        //TODO: Implement softdelete pattern!
//        //See: http://stackoverflow.com/questions/23260153/with-a-generic-repository-for-entity-framework-how-do-i-query-entities-which-im
//        // OR http://www.wiktorzychla.com/2013/10/soft-delete-pattern-for-entity.html

//        /// <summary>
//        /// The database context used.
//        /// </summary>
//        protected IApplicationDataContext InternalContext;

//        /// <summary>
//        /// The loaded set of entities.
//        /// </summary>
//        private readonly DbSet<TEntity> _dbSet;

//        /// <summary>
//        /// The IP-Address of the client connecting to the repository.
//        /// </summary>
//        private readonly string _clientIpAddress;

//        private bool _disposed;

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        protected GenericRepository()
//        {
//            //Apply the context
//            InternalContext = new TContext();

//            //Set the entity type for the current dbset.
//            _dbSet = InternalContext.Set<TEntity>();
//        }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="clientIpAddress">The IP Address of the client visiting the website</param>
//        protected GenericRepository(string clientIpAddress)
//        {
//            //Apply the context
//            InternalContext = new TContext();

//            //Set the entity type for the current dbset.
//            _dbSet = InternalContext.Set<TEntity>();

//            //Save IPAddress of the client
//            _clientIpAddress = clientIpAddress;
//        }

//        /// <summary>
//        /// The constructor taking the databasecontext.
//        /// </summary>
//        /// <param name="context">The databasecontext to use.</param>
//        /// <param name="clientIpAddress">The IP Address of the client using the context.</param>
//        protected GenericRepository(TContext context, string clientIpAddress)
//        {
//            //Apply the context
//            InternalContext = context;

//            //Set the entity type for the current dbset.
//            _dbSet = InternalContext.Set<TEntity>();

//            _clientIpAddress = clientIpAddress;
//        }

//        /// <summary>
//        /// Gets a single item based on the specified filter.
//        /// </summary>
//        /// <param name="filter">The expressions to filter for.</param>
//        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
//        /// <param name="includedProperties">Additional properties to be loaded eagerly.</param>
//        /// <returns>Returns the item matching the condition, otherwise null is returned.</returns>
//        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter, bool publishedItemsOnly = true, params Expression<Func<TEntity, object>>[] includedProperties)
//        {
//            var query = AsQueryable(publishedItemsOnly);
//            TEntity entity = null;

//            //Apply eager loading
//            foreach (Expression<Func<TEntity, object>> navigationProperty in includedProperties)
//            {
//                query = query.Include(navigationProperty);
//            }

//            if (filter != null)
//                entity = query.FirstOrDefault(filter);

//            return entity;
//        }

//        /// <summary>
//        /// Gets a single item based on the specified filter.
//        /// </summary>
//        /// <param name="id">The id of the object to find.</param>
//        /// <param name="includedProperties">Additional properties to be loaded eagerly.</param>
//        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
//        /// <returns>Returns the item matching the condition, otherwise null is returned.</returns>
//        public TEntity GetSingle(Guid id, bool publishedItemsOnly = true, params Expression<Func<TEntity, object>>[] includedProperties)
//        {
//            var query = AsQueryable(publishedItemsOnly);

//            //Apply eager loading
//            foreach (var navigationProperty in includedProperties)
//            {
//                query = query.Include(navigationProperty);
//            }

//            var entity = query.SingleOrDefault(o => o.Id == id);

//            return entity;
//        }

//        /// <summary>
//        /// Gets a single item based on the specified filter.
//        /// </summary>
//        /// <param name="filter">The expressions to filter for.</param>
//        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
//        /// <returns>Returns the item matching the condition, otherwise null is returned.</returns>
//        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> filter, bool publishedItemsOnly = true)
//        {
//            var query = AsQueryable(publishedItemsOnly);
//            TEntity entity = null;

//            if (filter != null)
//            {
//                entity = query.FirstOrDefault(filter);
//            }

//            return entity;
//        }

//        /// <summary>
//        /// Gets a list of items.
//        /// </summary>
//        /// <returns>A collection of entities matching the condition.</returns>
//        public virtual IEnumerable<TEntity> Get()
//        {
//            return Get(null, true);
//        }

//        /// <summary>
//        /// Gets a list of items matching the specified filter, order by and included properties.
//        /// </summary>
//        /// <param name="filter">The filter to apply.</param>
//        /// <returns>A collection of entities matching the condition.</returns>
//        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
//        {
//            return Get(filter, true);
//        }

//        /// <summary>
//        /// Returns an queryable object so own queries can be created.
//        /// </summary>
//        /// <param name="publishedItemsOnly">Include only published items?</param>
//        /// <returns>IQueryable of the specified type.</returns>
//        public IQueryable<TEntity> AsQueryable(bool publishedItemsOnly = true)
//        {
//            if (!publishedItemsOnly) return _dbSet;
//            try
//            {
//                return _dbSet.Where(IsPublished());
//            }
//            catch (Exception)
//            {
//                //TODO: Logging
//            }

//            return _dbSet;
//        }

//        /// <summary>
//        /// Returns an queryable object so own queries can be created.
//        /// </summary>
//        /// <param name="includedProperties">The properties to include using eager loading.</param>
//        /// <param name="publishedItemsOnly">Include only published items?</param>
//        /// <returns>IQueryable of the specified type.</returns>
//        public IQueryable<TEntity> AsQueryable(bool publishedItemsOnly = true, params Expression<Func<TEntity, object>>[] includedProperties)
//        {
//            var query = AsQueryable(publishedItemsOnly);

//            //Apply eager loading
//            foreach (var navigationProperty in includedProperties)
//            {
//                query = query.Include(navigationProperty);
//            }

//            return query;
//        }

//        /// <summary>
//        /// Gets a list of items matching the specified filter, order by and included properties.
//        /// </summary>
//        /// <param name="filter">The filter to apply.</param>
//        /// <param name="includedProperties">The properties to include to apply eager loading.</param>
//        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
//        /// <returns>A collection of entities matching the condition.</returns>
//        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, bool publishedItemsOnly, params Expression<Func<TEntity, object>>[] includedProperties)
//        {
//            var query = AsQueryable(publishedItemsOnly);

//            if (filter != null)
//            {
//                query = query.Where(filter);
//            }

//            //Apply eager loading
//            foreach (var navigationProperty in includedProperties)
//            {
//                query = query.Include(navigationProperty);
//            }

//            return query;
//        }

//        /// <summary>
//        /// Gets the object matching the provided systemname.
//        /// </summary>
//        /// <param name="systemName">The systemname to get the object for.</param>
//        /// <returns>The object if found, otherwise null is returned.</returns>
//        public TEntity GetBySystemName(string systemName)
//        {
//            return AsQueryable().SingleOrDefault(e => e.SystemName == systemName);
//        }

//        /// <summary>
//        /// Gets the object matching the id.
//        /// </summary>
//        /// <param name="id">The id to find the object for.</param>
//        /// <param name="publishedItemOnly"></param>
//        /// <returns>The matching object if found, otherwise null is returned.</returns>
//        public virtual TEntity GetById(Guid id, bool publishedItemOnly = true)
//        {
//            //return DbSet.Find(id);
//            return AsQueryable(publishedItemOnly).SingleOrDefault(e => e.Id.Equals(id));
//        }

//        /// <summary>
//        /// Inserts a new object to the database.
//        /// </summary>
//        /// <remarks>SaveChanges functions needs to be called after insertion.</remarks>
//        /// <param name="entity">The object to insert.</param>
//        public virtual void Insert(TEntity entity)
//        {
//            //if (entity.id == Guid.Empty) entity.id = Guid.NewGuid();

//            _dbSet.Add(entity);
//        }

//        /// <summary>
//        /// Updates the specified object in the database.
//        /// </summary>
//        /// <remarks>SaveChanges function needs to be called after updating.</remarks>
//        /// <param name="entityToUpdate"></param>
//        public virtual void Update(TEntity entityToUpdate)
//        {
//            _dbSet.Attach(entityToUpdate);
//            InternalContext.Entry(entityToUpdate).State = EntityState.Modified;
//        }

//        /// <summary>
//        /// Deletes the object matching the specified id.
//        /// </summary>
//        /// <remarks>SaveChanges functions needs to be called after deletion.</remarks>
//        /// <param name="id">The id to delete the object by.</param>
//        public virtual void Delete( /*object id*/ Guid id)
//        {
//            var entityToDelete = _dbSet.Find(id);
//            Delete(entityToDelete);
//        }

//        /// <summary>
//        /// Deletes the provided object from the database.
//        /// </summary>
//        /// <remarks>SaveChanges functions needs to be called after deletion.</remarks>
//        /// <param name="entityToDelete">The object to delete.</param>
//        public virtual void Delete(TEntity entityToDelete)
//        {
//            if (InternalContext.Entry(entityToDelete).State == EntityState.Detached)
//            {
//                _dbSet.Attach(entityToDelete);
//            }
//            InternalContext.Entry(entityToDelete).State = EntityState.Deleted;
//            _dbSet.Remove(entityToDelete);
//        }


//        /// <summary>
//        /// Is the item published?
//        /// There are several conditions like publication-date checked to validate wether a page is published or not.
//        /// </summary>
//        /// <returns>Expression which can be executed to check if a page is published or not.</returns>
//        protected virtual Expression<Func<TEntity, bool>> IsPublished()
//        {
//            var dateTimeNow = DateTimeHelper.DateTimeNow();
//            return
//                c =>
//                    !c.IsDeleted && c.IsPublished && c.PublishStart <= dateTimeNow &&
//                    (!c.PublishEnd.HasValue || c.PublishEnd > dateTimeNow);
//        }

//        /// <summary>
//        /// SaveChanges's all pending changes on the context to the database.
//        /// </summary>
//        public void SaveChanges()
//        {
//            InternalContext.SaveChanges(_clientIpAddress);
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (_disposed)
//                return;

//            if (disposing)
//            {
//                if (InternalContext != null)
//                {
//                    InternalContext.Dispose();
//                    InternalContext = null;
//                }
//                _disposed = true;
//            }
//        }

//    }
//}
