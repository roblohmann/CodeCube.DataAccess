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
        /// When was the object created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Who created the object?
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// When was the object last changed?
        /// <remarks>Can be NULL if no changes has been made yet.</remarks>
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Who last changed the object?
        /// <remarks>Can be NULL if no changes has been made yet.</remarks>
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Is the object in a deleted state?
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// When was the object deleted?
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Who did the delete action on this object?
        /// </summary>
        public string DeletedBy { get; set; }

        /// <summary>
        /// Used to prevent concurrency exceptions.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
