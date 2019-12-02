using System;
using System.Linq;
using System.Reflection;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Extensions
{
    public static class EntityFrameworkFilterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="https://stackoverflow.com/a/45097532/291293"/>
        /// <param name="modelBuilder"></param>
        /// <param name="entityType"></param>
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                    .Invoke(null, new object[] { modelBuilder });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="https://stackoverflow.com/a/45097532/291293"/>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder) where TEntity : class, ISoftDelete
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }

        #region privates
        private static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EntityFrameworkFilterExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");
        #endregion
    }
}
