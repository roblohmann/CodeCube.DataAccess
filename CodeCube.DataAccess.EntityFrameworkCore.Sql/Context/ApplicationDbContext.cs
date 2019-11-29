using System;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        /// <summary>
        /// Deletes the entity with the specified id.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        public void Remove<TEntity>(Guid id) where TEntity : BaseEntity, new()
        {
            TEntity objectToRemove = new TEntity { Id = id };

            Attach(objectToRemove);
            Remove(objectToRemove);
        }
    }
}