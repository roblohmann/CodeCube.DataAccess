using System;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Interfaces
{
    /// <summary>
    /// Inherit your entities from this interface if you want to enable fields used for changetracking.
    /// Eg. Createdby, CreatedOn, UpdatedBy and UpdatedOn
    /// </summary>
    public interface IChangeTracking
    {
        /// <summary>
        /// When was the object created.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Who created the object?
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// When was the object last changed?
        /// <remarks>Can be NULL if no changes has been made yet.</remarks>
        /// </summary>
        DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Who last changed the object?
        /// <remarks>Can be NULL if no changes has been made yet.</remarks>
        /// </summary>
        string UpdatedBy { get; set; }
    }
}
