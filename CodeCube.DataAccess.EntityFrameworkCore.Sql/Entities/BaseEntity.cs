using System;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Entities
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
        public byte[] RowVersion { get; set; }
    }
}
