﻿using SOURCES.Builders.Abstract;
using SOURCES.Models;
using SOURCES.Workers;

namespace SOURCES.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IRepositoryBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.IRepositoryPath, $"I{model.Name}Repository.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using DAL.EntityFramework.GenericRepository;
                   using ENTITIES.Entities{entityPath};

                   namespace DAL.EntityFramework.Abstract;

                   public interface I{entityName}Repository : IGenericRepository<{entityName}>
                   {
                   }

                   """;
        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}