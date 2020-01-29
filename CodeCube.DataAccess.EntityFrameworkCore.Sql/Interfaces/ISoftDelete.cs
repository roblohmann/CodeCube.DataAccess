using System;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Interfaces
{
    /// <summary>
    /// Inherit from this interface on your entity to add properties used for soft-deletion.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Is the object deleted or not?
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// When was the object deleted?
        /// </summary>
        DateTime? DeletedOn { get; set; }
    }
}
