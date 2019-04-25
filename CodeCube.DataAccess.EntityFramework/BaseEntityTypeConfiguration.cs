using CodeCube.DataAccess.EntityFramework.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CodeCube.DataAccess.EntityFramework
{
    public class BaseEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : class, IBaseEntity
    {
        public BaseEntityTypeConfiguration()
        {
            Property(t => t.TimeStamp).IsRowVersion().IsConcurrencyToken();
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.PublishStart).IsRequired();
            Property(t => t.PublishEnd).IsOptional();
            Property(t => t.DateCreated).IsRequired();
            Property(t => t.DateDeleted).IsOptional();
            Property(t => t.IsPublished).IsRequired();
            Property(t => t.IsDeleted).IsRequired();

            Property(t => t.SystemName).HasMaxLength(150).IsRequired();
        }
    }
}
