using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http;
using Oqtane.Models;
using System;
using System.Linq;

namespace OE.TenTrees.Repository.Extensions
{
    public static class AuditableExtensions
    {
        public static void SetAuditFields(this DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
            var currentTime = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUser;
                        entry.Entity.CreatedOn = currentTime;
                        entry.Entity.ModifiedBy = currentUser;
                        entry.Entity.ModifiedOn = currentTime;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = currentUser;
                        entry.Entity.ModifiedOn = currentTime;
                        // Ensure CreatedBy and CreatedOn are not modified
                        entry.Property(p => p.CreatedBy).IsModified = false;
                        entry.Property(p => p.CreatedOn).IsModified = false;
                        break;
                }
            }
        }

        public static int SaveChangesWithAudit(this DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            context.SetAuditFields(httpContextAccessor);
            return context.SaveChanges();
        }
    }
}