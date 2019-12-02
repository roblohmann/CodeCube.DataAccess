using System;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
