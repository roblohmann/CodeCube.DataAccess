using System;
using System.Linq;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Constants;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Entities;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Extensions;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Context
{
    public abstract class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Deletes the entity with the specified id.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public virtual void RemoveById<TEntity>(Guid id) where TEntity : BaseEntity, new()
        {
            var objectToRemove = Find<TEntity>(id);
            Remove(objectToRemove);
        }

        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention from the entity types
        ///     exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        ///     and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        ///     then this method will not be run.
        ///     When overridden, don't forget to call base.OnModelCreating if you want the softdelete to keep working.
        /// </remarks>
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other extensions) typically
        ///     define extension methods on this object that allow you to configure aspects of the model that are specific
        ///     to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new InvalidOperationException(ErrorConstants.ModelbuilderRequired);

            var entitiesWithSoftdelete = modelBuilder.Model.GetEntityTypes().Where(t => typeof(ISoftDelete).IsAssignableFrom(t.ClrType));
            foreach (var entityType in entitiesWithSoftdelete)
            {
                modelBuilder.SetSoftDeleteFilter(entityType.ClrType);
            }
        }
    }
}