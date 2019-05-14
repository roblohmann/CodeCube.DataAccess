using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Extensions
{
    /// <summary>
    /// Class for DbContext extensions
    /// </summary>
    internal static class DbContextExtension
    {
        /// <summary>
        /// Are all available migrations applied to this DbContext?
        /// </summary>
        /// <param name="dbContext">The DbContext.</param>
        /// <returns>True if all migrations are applied, otherwise false.</returns>
        internal static bool AllMigrationsApplied(this DbContext dbContext)
        {
            var applied = dbContext.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = dbContext.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }
    }
}
