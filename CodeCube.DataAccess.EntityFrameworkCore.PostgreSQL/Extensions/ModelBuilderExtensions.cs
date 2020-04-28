using CodeCube.DataAccess.EntityFrameworkCore.Entities.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace CodeCube.DataAccess.EntityFrameworkCore.PostgreSQL.Extensions
{
    /// <summary>
    /// Class with extension methods on the EF Core ModelBuilder.
    /// <emarks>PostgreSQL only!</emarks>
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Define the Id property of the specified <typeparamref name="TEntity"/> as an UUID / Guid in the database.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="modelBuilder">The EF Core ModelBuilder</param>
        public static void HasKeyUniqueIdentifier<TEntity>(this ModelBuilder modelBuilder) where TEntity : BaseEntity
        {
            modelBuilder.HasPostgresExtension("uuid-ossp").Entity<TEntity>().Property(t => t.Id).HasDefaultValueSql("uuid_generate_v4()");
        }
    }
}
