using System;
using System.Collections.Generic;
using System.IO;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Constants;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.ContextFactory
{
    public abstract class DesignTimeDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext
    {
        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="args"> Arguments provided by the design-time service. </param>
        /// <returns> An instance of ApplicationDbContext.</returns>
        public T CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

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
        public T CreateDbContext(string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new KeyNotFoundException(ErrorConstants.MissingConnectionstring);
            }

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(connectionstring);

            var dbContext = (T)Activator.CreateInstance(typeof(T),builder.Options);

            return dbContext;
        }
    }
}
