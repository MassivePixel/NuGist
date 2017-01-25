using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NuGist.Model;
using System;
using System.Collections.Generic;

namespace NuGist.Web
{
    public static class DbSetExtensions
    {
        public static EntityEntry<T> AddNewEntity<T>(this DbSet<T> set, T entity, string userId)
            where T : BaseEntity
        {
            entity.CreatedOn = entity.ModifiedOn = DateTimeOffset.UtcNow;
            entity.CreatedById = entity.ModifiedById = userId;
            return set.Add(entity);
        }

        public static void UpdateModified(this BaseEntity entity, string userId = null)
        {
            entity.ModifiedOn = DateTimeOffset.UtcNow;
            entity.ModifiedById = userId;
        }
        public static void UpdateModified(this IEnumerable<BaseEntity> entities, string userId = null)
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var entity in entities)
            {
                entity.ModifiedOn = now;
                entity.ModifiedById = userId;
            }
        }
    }
}
