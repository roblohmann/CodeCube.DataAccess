namespace CodeCube.DataAccess.EntityFrameworkCore.Interfaces
{
    /// <summary>
    /// Inherit your entities from this interface if you want to enable fields used for changetracking.
    /// Eg. Createdby, CreatedOn, UpdatedBy and UpdatedOn
    /// </summary>
    public interface IChangeTracking : IUpdatedTracking, ICreatedTracking
    {

    }
}
