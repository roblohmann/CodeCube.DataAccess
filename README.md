# CodeCube.DataAccess
Library with repository pattern and helpers around Entity Framework and Entity Framework Core

![Nuget](https://img.shields.io/nuget/dt/CodeCube.DataAccess.EntityFramework?style=for-the-badge)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/CodeCube.DataAccess.EntityFramework?style=for-the-badge)
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