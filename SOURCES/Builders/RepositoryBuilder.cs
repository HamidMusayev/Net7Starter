﻿using SOURCES.Builders.Abstract;
using SOURCES.Models;
using SOURCES.Workers;

namespace SOURCES.Builders;

// ReSharper disable once UnusedType.Global
public class RepositoryBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.RepositoryPath, $"{model.Name}Repository.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using DAL.EntityFramework.Abstract;
                   using DAL.EntityFramework.Context;
                   using DAL.EntityFramework.GenericRepository;
                   using ENTITIES.Entities{entityPath};

                   namespace DAL.EntityFramework.Concrete;

                   public class {entityName}Repository : GenericRepository<{entityName}>, I{entityName}Repository
                   {
                       private readonly DataContext _dataContext;
                   
                       public {entityName}Repository(DataContext dataContext)
                           : base(dataContext)
                       {
                           _dataContext = dataContext;
                       }
                   }

                   """;
        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}