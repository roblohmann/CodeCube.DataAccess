using System;
using System.Collections.Generic;
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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    //.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new KeyNotFoundException(ErrorConstants.MissingConnectionstring);
            }

            return CreateDbContext(connectionString, args);
        }

        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="connectionstring">The connectionstring to the database.</param>
        /// <param name="args"> Arguments provided by the design-time service. </param>
        /// <returns> An instance of ApplicationDbContext.</returns>
        public T CreateDbContext(string connectionstring, string[] args)
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
