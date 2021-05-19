using System;
using System.ComponentModel.DataAnnotations;

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
        public byte[] RowVersion { get; set; }
    }
}
