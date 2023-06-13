﻿using System.Linq.Expressions;
using ENTITIES.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.EntityFramework.DatabaseContext;

public static class ContextExtensions
{
    public static void AddGlobalFilter(this ModelBuilder modelBuilder, string property, object value)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(Auditable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var deletedCheck = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, property),
                        Expression.Constant(value)
                    ), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }
    }
}