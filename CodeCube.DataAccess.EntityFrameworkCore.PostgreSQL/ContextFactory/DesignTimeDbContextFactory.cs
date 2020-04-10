using System;
using System.Collections.Generic;
using CodeCube.DataAccess.EntityFrameworkCore.PostgreSQL.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CodeCube.DataAccess.EntityFrameworkCore.PostgreSQL.ContextFactory
{
    /// <summary>
    ///     A factory for creating derived <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> instances. Inherit you context from this class to enable
    ///     design-time services for context types that do not have a public default constructor. At design-time,
    ///     derived <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> instances can be created in order to enable specific design-time
    ///     experiences such as Migrations. Design-time services will automatically discover implementations of
    ///     this interface that are in the startup assembly or the same assembly as the derived context.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="args"> Arguments provided by the design-time service. </param>
        /// <returns> An instance of ApplicationDbContext.</returns>
        public TContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = GetAppConfiguration();

            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new KeyNotFoundException(ErrorConstants.MissingConnectionstring);
            }

            return CreateDbContext(connectionString);
        }

        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="connectionstring">The connectionstring to the database.</param>
        /// <returns> An instance of ApplicationDbContext.</returns>
        public TContext CreateDbContext(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new KeyNotFoundException(ErrorConstants.MissingConnectionstring);
            }

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseSqlServer(connectionstring);

            var dbContext = (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);

            return dbContext;
        }

        #region privates
        private static IConfiguration GetAppConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
        #endregion
    }
}
