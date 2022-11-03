using CodeCube.DataAccess.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using CodeCube.Core.Enumerations;
using CodeCube.Core.Helpers;

namespace CodeCube.DataAccess.EntityFramework
{
    public abstract class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();

        #region constructor
        protected ApplicationDbContext(string connectionstring = "name=ApplicationConnectionstring") 
                : base(connectionstring)
        {
            InitEntityFrameworkConfiguration();
        }
        #endregion

        /// <summary>
        /// Gets a DbEntityEntry object for the given entity providing access 
        /// to information about the entity and the ability to perform actions on the entity.
        /// </summary>
        /// <param name="dbEntity">The database entity</param>
        /// <returns></returns>
        public DbEntityEntry Entry(IBaseEntity dbEntity)
        {
            return base.Entry(dbEntity);
        }

        /// <summary>
        /// Method to save stuff to the database.
        /// </summary>F
        public override int SaveChanges()
        {
            return SaveChanges(null);
        }

        /// <summary>
        /// Override this method so created and modified dates are set dynamically.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges(string clientIpAddress)
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted);

            foreach (var entry in modified)
            {
                switch (entry.Entity)
                {
                    case IChangeTracker changedOrAddedItem:
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                changedOrAddedItem.DateCreated = DateTimeHelper.DateTimeNow(EDateTimeRegion.WesternEurope);
                                changedOrAddedItem.DateModified = DateTimeHelper.DateTimeNow(EDateTimeRegion.WesternEurope);
                                break;
                            case EntityState.Modified:
                                changedOrAddedItem.DateModified = DateTimeHelper.DateTimeNow(EDateTimeRegion.WesternEurope);
                                break;
                            case EntityState.Deleted:
                                changedOrAddedItem.DateDeleted = DateTimeHelper.DateTimeNow(EDateTimeRegion.WesternEurope);
                                changedOrAddedItem.DateModified = DateTimeHelper.DateTimeNow(EDateTimeRegion.WesternEurope);

                                //Call softdelete method
                                SoftDelete(entry);
                                break;
                        }
                        break;
                    case IIpTracking ipTrackedItem when !string.IsNullOrWhiteSpace(clientIpAddress):
                        ipTrackedItem.IpAddress = clientIpAddress;
                        break;
                }
            }

            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException dex)
            {
                LogError($"DbUpdateException occured! {dex.GetBaseException().Message}");

                throw;
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add($"{DateTimeHelper.DateTimeNow(EDateTimeRegion.WesternEurope)}: Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");

                    outputLines.AddRange(eve.ValidationErrors.Select(ve => $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\""));
                }

                LogError(outputLines.ToString());
            }

            return 0;
        }

        public abstract void LogError(string errorMessage);

        #region private methods
        /// <summary>
        /// Handles the delete and soft-deletes the entry.
        /// </summary>
        /// <see cref="http://www.wiktorzychla.com/2013/10/soft-delete-pattern-for-entity.html"/>
        /// <param name="entry"></param>
        private void SoftDelete(DbEntityEntry entry)
        {
            //Get the type
            var entryEntityType = entry.Entity.GetType();

            //Get the tablename base on the type.
            var tableName = GetTableName(entryEntityType);

            //Get the primarykeyname based on the type.
            var primaryKeyName = GetPrimaryKeyName(entryEntityType);

            //Format the query.
            var deletequery = $"UPDATE {tableName} SET IsDeleted = 1 WHERE {primaryKeyName} = @id";

            //Execute the sql query
            Database.ExecuteSqlCommand(deletequery, new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));

            //Marking it Unchanged prevents the hard delete
            //entry.State = EntityState.Unchanged;
            //So does setting it to Detached:
            //And that is what EF does when it deletes an item
            //http://msdn.microsoft.com/en-us/data/jj592676.aspx
            entry.State = EntityState.Detached;
        }

        /// <summary>
        /// Get the entityset bases on the provided type.
        /// </summary>
        /// <param name="type">The type of entity.</param>
        /// <returns>The entityset.</returns>
        private EntitySetBase GetEntitySet(Type type)
        {
            //Check if the cache contains the entity type.
            if (_mappingCache.ContainsKey(type)) return _mappingCache[type];

            var octx = ((IObjectContextAdapter)this).ObjectContext;

            var typeName = System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(type).Name;

            var entitySet = octx.MetadataWorkspace
                .GetItemCollection(DataSpace.SSpace)
                .GetItems<EntityContainer>()
                .SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName))
                .FirstOrDefault();

            //If the entityset could not be found, report exception.
            if (entitySet == null)
                throw new ArgumentException(@"Entity type not found in GetTableName", typeName);

            //Add the type to the cache.
            _mappingCache.Add(type, entitySet);

            //Return the item from the cache.
            return _mappingCache[type];
        }

        /// <summary>
        /// Get the tablename.
        /// </summary>
        /// <param name="type">The type of entity.</param>
        /// <returns>A formatted string representing the table name.</returns>
        private string GetTableName(Type type)
        {
            var entitySet = GetEntitySet(type);

            return $"[{entitySet.MetadataProperties["Schema"].Value}].[{entitySet.MetadataProperties["Table"].Value}]";
        }

        /// <summary>
        /// Get the primary key name.
        /// </summary>
        /// <param name="type">The type of entity.</param>
        /// <returns>The name of the primary key.</returns>
        private string GetPrimaryKeyName(Type type)
        {
            var entitySet = GetEntitySet(type);

            return entitySet.ElementType.KeyMembers[0].Name;
        }

        private void InitEntityFrameworkConfiguration()
        {
            //Disable lazy loading by default!
            Configuration.LazyLoadingEnabled = false;

            //Disable change tracker! We're handling this ourselves!
            Configuration.AutoDetectChangesEnabled = false;

            //Disable dynamic proxy generation for entity framework.
            Configuration.ProxyCreationEnabled = false;

            //Clear all initializers
            Database.SetInitializer<ApplicationDbContext>(null);
        }
        #endregion
    }
}
