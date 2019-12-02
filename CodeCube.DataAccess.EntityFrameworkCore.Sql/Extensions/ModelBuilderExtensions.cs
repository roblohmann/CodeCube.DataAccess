using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CodeCube.DataAccess.EntityFrameworkCore.Sql.Interfaces;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Extensions
{
    //Code taken from Phil Haacked > https://haacked.com/archive/2019/07/29/query-filter-by-interface/
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Apply the specified expression for the matching interface on all entities
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filterExpression">The filter expression to apply.</param>
        /// <typeparam name="TEntityInterface"></typeparam>
        public static void SetQueryFilterOnAllEntities<TEntityInterface>(this ModelBuilder builder, Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            var entityTypes = builder.Model.GetEntityTypes()
                                .Where(t => t.BaseType == null)
                                .Select(t => t.ClrType)
                                .Where(t => typeof(TEntityInterface).IsAssignableFrom(t));

            foreach (var type in entityTypes)
            {
                builder.SetEntityQueryFilter(type, filterExpression);
            }
        }
        private static void SetQueryFilter<TEntity, TEntityInterface>(this ModelBuilder builder, Expression<Func<TEntityInterface, bool>> filterExpression)
                where TEntityInterface : class
                where TEntity : class, TEntityInterface
        {
            var concreteExpression = filterExpression.Convert<TEntityInterface, TEntity>();
            builder.Entity<TEntity>().HasQueryFilter(concreteExpression);
        }

        private static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtensions)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));

        private static void SetEntityQueryFilter<TEntityInterface>(this ModelBuilder builder, Type entityType, Expression<Func<TEntityInterface, bool>> filterExpression)
        {
            SetQueryFilterMethod
                    .MakeGenericMethod(entityType, typeof(TEntityInterface))
                    .Invoke(null, new object[] { builder, filterExpression });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityType"></param>
        /// <remarks>This code has been kindly borrowed from https://stackoverflow.com/a/45097532/291293</remarks>
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType).Invoke(null, new object[] { modelBuilder });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <remarks>This code has been kindly borrowed from https://stackoverflow.com/a/45097532/291293</remarks>
        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder) where TEntity : class, ISoftDelete
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }

        #region privates
        private static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(ModelBuilderExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");
        #endregion
    }
}
