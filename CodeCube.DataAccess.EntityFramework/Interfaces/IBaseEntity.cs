using System;

namespace CodeCube.DataAccess.EntityFramework.Interfaces
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTime PublishStart { get; set; }

        DateTime? PublishEnd { get; set; }

        DateTime DateCreated { get; set; }

        DateTime? DateModified { get; set; }

        DateTime? DateDeleted { get; set; }

        string SystemName { get; set; }

        bool IsPublished { get; set; }

        bool IsDeleted { get; set; }

        byte[] TimeStamp { get; set; }
    }
}
