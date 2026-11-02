# CodeCube.DataAccess
Library with repository pattern and helpers around Entity Framework Core (SQL Server and PostgreSQL).

![Nuget](https://img.shields.io/nuget/dt/CodeCube.DataAccess.EntityFrameworkCore?style=for-the-badge)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/CodeCube.DataAccess.EntityFrameworkCore?style=for-the-badge)
![GitHub](https://img.shields.io/github/license/roblohmann/CodeCube.DataAccess?style=for-the-badge)


Set query filter on all entities.
``` C#   
    modelBuilder.SetQueryFilterOnAllEntities<ISoftDelete>(e => !e.IsDeleted);
```


``` C#   
    var entitiesWithSoftdelete = modelBuilder.Model.GetEntityTypes().Where(t => typeof(ISoftDelete).IsAssignableFrom(t.ClrType));
    foreach (var entityType in entitiesWithSoftdelete)
    {
        modelBuilder.SetSoftDeleteFilter(entityType.ClrType);
    }
```