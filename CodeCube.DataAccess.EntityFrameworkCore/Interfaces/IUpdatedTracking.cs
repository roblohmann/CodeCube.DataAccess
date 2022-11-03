using System;

namespace CodeCube.DataAccess.EntityFrameworkCore.Interfaces
{
    /// <summary>
    /// Use this interface if you want the fields UpdatedOn and Updatedby to track who updates your entity and when.
    /// </summary>
    public interface IUpdatedTracking
    {
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
