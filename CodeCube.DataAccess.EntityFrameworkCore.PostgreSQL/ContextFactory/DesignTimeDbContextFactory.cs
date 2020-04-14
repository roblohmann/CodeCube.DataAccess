using System;
using System.Collections.Generic;
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
        public virtual TContext CreateDbContext(string[] args)
        {
            var connectionstring = GetConnectionstring();

            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                IConfiguration configuration = GetAppConfiguration();
                connectionstring = configuration.GetConnectionString("DatabaseConnection");
            }            

            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new KeyNotFoundException(Constants.ErrorConstants.MissingConnectionstring);
            }

            return CreateDbContext(connectionstring);
        }

        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="connectionstring">The connectionstring to the database.</param>
        /// <returns> An instance of ApplicationDbContext.</returns>
        public virtual TContext CreateDbContext(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new KeyNotFoundException(Constants.ErrorConstants.ConnectionstringParameterRequired);
            }

            var builder = new DbContextOptionsBuilder<TContext>();
            builder.UseNpgsql(connectionstring);

            var dbContext = (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);

            return dbContext;
        }

        /// <summary>
        /// Method to retrieve the connectionstring. This is used by the factory to get the connectionstring while running migrations.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetConnectionstring();

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
