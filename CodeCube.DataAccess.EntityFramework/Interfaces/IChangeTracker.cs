using System;

namespace CodeCube.DataAccess.EntityFramework.Interfaces
{
    public interface IChangeTracker
    {
        /// <summary> 
        /// Gets or sets the date the object was created. 
        /// </summary> 
        DateTime DateCreated { get; set; }

        /// <summary>
        /// The id of the person who created the object.
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary> 
        /// Gets or sets the date the object was last modified. 
        /// </summary> 
        DateTime? DateModified { get; set; }

        /// <summary>
        /// The id of the person who last changed the object.
        /// </summary>
        string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date the object was removed.
        /// </summary>
        DateTime? DateDeleted { get; set; }

        /// <summary>
        /// The id of the person who deleted the object.
        /// </summary>
        string DeletedBy { get; set; }
    }
}
