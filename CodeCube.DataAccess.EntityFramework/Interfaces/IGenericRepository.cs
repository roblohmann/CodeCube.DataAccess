using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CodeCube.DataAccess.EntityFramework.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets the object matching the id.
        /// </summary>
        /// <param name="id">The id to find the object for.</param>
        /// <param name="publishedItemOnly"></param>
        /// <returns>The matching object if found, otherwise null is returned.</returns>
        TEntity GetById(Guid id, bool publishedItemOnly = true);

        /// <summary>
        /// Gets the object matching the provided systemname.
        /// </summary>
        /// <param name="systemName">The systemname to get the object for.</param>
        /// <returns>The object if found, otherwise null is returned.</returns>
        TEntity GetBySystemName(string systemName);

        /// <summary>
        /// Gets a single item based on the specified filter.
        /// </summary>
        /// <param name="filter">The expressions to filter for.</param>
        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
        /// <returns>Returns the item matching the condition, otherwise null is returned.</returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> filter = null, bool publishedItemsOnly = true);

        /// <summary>
        /// Gets a single item based on the specified filter.
        /// </summary>
        /// <param name="filter">The expressions to filter for.</param>
        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
        /// <param name="includedProperties">Additional properties to be loaded eagerly.</param>
        /// <returns>Returns the item matching the condition, otherwise null is returned.</returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> filter, bool publishedItemsOnly = true, params Expression<Func<TEntity, object>>[] includedProperties);

        /// <summary>
        /// Gets a single item based on the specified filter.
        /// </summary>
        /// <param name="id">The id of the object to find.</param>
        /// <param name="includedProperties">Additional properties to be loaded eagerly.</param>
        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
        /// <returns>Returns the item matching the condition, otherwise null is returned.</returns>
        TEntity GetSingle(Guid id, bool publishedItemsOnly = true, params Expression<Func<TEntity, object>>[] includedProperties);

        /// <summary>
        /// Gets a list of items.
        /// </summary>
        /// <returns>A collection of entities matching the condition.</returns>
        IEnumerable<TEntity> Get();

        /// <summary>
        /// Gets a list of items matching the specified filter, order by and included properties.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A collection of entities matching the condition.</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Gets a list of items matching the specified filter, order by and included properties.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="publishedItemsOnly">True if only publish and active items should be included, otherwise false.</param>
        /// <param name="includedProperties">The properties to include to apply eager loading.</param>
        /// <returns>A collection of entities matching the condition.</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, bool publishedItemsOnly, params Expression<Func<TEntity, object>>[] includedProperties);

        /// <summary>
        /// Returns an queryable object so own queries can be created.
        /// </summary>
        /// <param name="publishedItemsOnly">Include only published items?</param>
        /// /// <param name="includedProperties">The properties to include using eager loading.</param>
        /// <returns>IQueryable of the specified type.</returns>
        IQueryable<TEntity> AsQueryable(bool publishedItemsOnly = true, params Expression<Func<TEntity, object>>[] includedProperties);

        /// <summary>
        /// Inserts a new object to the database.
        /// </summary>
        /// <remarks>SaveChanges functions needs to be called after insertion.</remarks>
        /// <param name="entity">The object to insert.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Updates the specified object in the database.
        /// </summary>
        /// <remarks>SaveChanges function needs to be called after updating.</remarks>
        /// <param name="entityToUpdate"></param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Deletes the object matching the specified id.
        /// </summary>
        /// <remarks>SaveChanges functions needs to be called after deletion.</remarks>
        /// <param name="id">The id to delete the object by.</param>
        void Delete(Guid id);

        /// <summary>
        /// Deletes the provided object from the database.
        /// </summary>
        /// <remarks>SaveChanges functions needs to be called after deletion.</remarks>
        /// <param name="entityToDelete">The object to delete.</param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// SaveChanges's all pending changes on the context to the database.
        /// </summary>
        void SaveChanges();
    }
}
