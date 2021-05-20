using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeCube.DataAccess.EntityFrameworkCore.Entities.SQL
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// The primary key. Unique for all objects.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Used to prevent concurrency exceptions.
        /// </summary>
        [ConcurrencyCheck]
        [Obsolete("This property will be removed in a future version! Please use 'RowVersion' property!",false)]
        public byte[] ConcurrencyValidationVersion { get; set; }

        /// <summary>
        /// Used to prevent concurrency exceptions.
        /// </summary>
        [ConcurrencyCheck]
        [Column("xmin", TypeName = "xid")]
        public long RowVersion { get; set; }
    }
}
