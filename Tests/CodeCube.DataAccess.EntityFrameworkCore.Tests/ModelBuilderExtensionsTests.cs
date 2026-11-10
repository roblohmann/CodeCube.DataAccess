using System;
using System.Linq;
using CodeCube.DataAccess.EntityFrameworkCore.Extensions;
using CodeCube.DataAccess.EntityFrameworkCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CodeCube.DataAccess.EntityFrameworkCore.Tests
{
    public class ModelBuilderExtensionsTests
    {
        public class Document : ISoftDelete
        {
            public int Id { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime? DeletedOn { get; set; }
        }

        public class Note
        {
            public int Id { get; set; }
        }

        private sealed class TestContext : DbContext
        {
            private readonly Action<ModelBuilder> _configure;

            public TestContext(string databaseName, Action<ModelBuilder> configure)
                : base(new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase(databaseName).Options)
            {
                _configure = configure;
            }

            public DbSet<Document> Documents => Set<Document>();
            public DbSet<Note> Notes => Set<Note>();

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Document>();
                modelBuilder.Entity<Note>();
                _configure(modelBuilder);
            }
        }

        private static TestContext CreateContext(Action<ModelBuilder> configure)
        {
            return new TestContext(Guid.NewGuid().ToString(), configure);
        }

        [Fact]
        public void SetQueryFilterOnAllEntities_FiltersOnlyEntitiesImplementingTheInterface()
        {
            using var context = CreateContext(b => b.SetQueryFilterOnAllEntities<ISoftDelete>(x => !x.IsDeleted));
            context.Documents.AddRange(new Document { Id = 1 }, new Document { Id = 2, IsDeleted = true });
            context.Notes.AddRange(new Note { Id = 1 }, new Note { Id = 2 });
            context.SaveChanges();

            Assert.Equal(new[] { 1 }, context.Documents.Select(d => d.Id).ToArray());
            Assert.Equal(2, context.Notes.Count());
        }

        [Fact]
        public void SetQueryFilterOnAllEntities_CanBeBypassedWithIgnoreQueryFilters()
        {
            using var context = CreateContext(b => b.SetQueryFilterOnAllEntities<ISoftDelete>(x => !x.IsDeleted));
            context.Documents.AddRange(new Document { Id = 1 }, new Document { Id = 2, IsDeleted = true });
            context.SaveChanges();

            Assert.Equal(2, context.Documents.IgnoreQueryFilters().Count());
        }

        [Fact]
        public void SetSoftDeleteFilter_Generic_HidesDeletedEntities()
        {
            using var context = CreateContext(b => b.SetSoftDeleteFilter<Document>());
            context.Documents.AddRange(new Document { Id = 1 }, new Document { Id = 2, IsDeleted = true });
            context.SaveChanges();

            Assert.Equal(new[] { 1 }, context.Documents.Select(d => d.Id).ToArray());
        }

        [Fact]
        public void SetSoftDeleteFilter_WithRuntimeType_HidesDeletedEntities()
        {
            using var context = CreateContext(b => b.SetSoftDeleteFilter(typeof(Document)));
            context.Documents.AddRange(new Document { Id = 1 }, new Document { Id = 2, IsDeleted = true });
            context.SaveChanges();

            Assert.Equal(new[] { 1 }, context.Documents.Select(d => d.Id).ToArray());
        }
    }
}
