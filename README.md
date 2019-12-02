# CodeCube.DataAccess
Library with repository pattern and helpers around Entity Framework and Entity Framework Core

[![NuGet](https://img.shields.io/nuget/vpre/CodeCube.DataAccess.EntityFramework.svg)](https://www.nuget.org/packages/CodeCube.DataAccess.EntityFramework)
[![NuGet](https://img.shields.io/nuget/dt/CodeCube.DataAccess.EntityFramework.svg)](https://www.nuget.org/packages/CodeCube.DataAccess.EntityFramework) 


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
