using System;

namespace CodeCube.DataAccess.EntityFrameworkCore.Interfaces
{
    /// <summary>
    /// Use this interface if you want the fields UpdatedOn and Updatedby to track who updates your entity and when.
    /// </summary>
    public interface ICreatedTracking
    {
        /// <summary>
        /// When was the object created.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Who created the object?
        /// </summary>
        string CreatedBy { get; set; }
    }
}