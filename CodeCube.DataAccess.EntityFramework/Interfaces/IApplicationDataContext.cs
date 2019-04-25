using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CodeCube.DataAccess.EntityFramework.Interfaces
{
    public interface IApplicationDataContext : IDisposable
    {
        /// <summary>
        /// Returns a DbSet instance for access to entities of the given type
        /// in the context and the underlying database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Gets a DbEntityEntry object for the given entity providing access 
        /// to information about the entity and the ability to perform actions on the entity.
        /// </summary>
        /// <param name="dbEntity">The database entity</param>
        /// <returns></returns>
        DbEntityEntry Entry(IBaseEntity dbEntity);

        /// <summary>
        /// Method to save stuff to the database.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Method to save stuff to the database.
        /// The client ip-address is used for object implementing IIpTracking
        /// </summary>
        /// <param name="clientIpAddress">The ip-address of the connected client.</param>
        int SaveChanges(string clientIpAddress);

        /// <summary>
        /// Log errors coming from the datacontext.
        /// </summary>
        /// <param name="errorMessage"></param>
        void LogError(string errorMessage);
    }
}
