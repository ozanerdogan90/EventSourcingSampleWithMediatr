using EventSourcingSampleWithCQRSandMediatr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<Audit> Audits { get; set; }

        public Context(DbContextOptions<Context> options)
       : base(options)
        {

        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();
            var changedEntries = ChangeTracker.Entries().Where(x => !(x.Entity is Audit));
            var auditList = new List<Audit>();

            var addedEntities = changedEntries.Where(x => x.State == EntityState.Added);
            foreach (var entity in addedEntities)
            {
                if (entity.Entity is BaseEntity baseEntity)
                {
                    baseEntity.Id = Guid.NewGuid();
                    baseEntity.CreatedDate = DateTime.Now;
                    auditList.Add(new AuditEntry(entity).ToAudit());
                }
            }

            var updatedEntities = changedEntries.Where(x => x.State == EntityState.Modified);
            foreach (var entity in updatedEntities)
            {
                if (entity.Entity is BaseEntity baseEntity)
                {
                    baseEntity.UpdatedDate = DateTime.Now;
                    auditList.Add(new AuditEntry(entity).ToAudit());
                }
            }

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            if (auditList.Any())
            {
                Audits.AddRange(auditList);
                await base.SaveChangesAsync();
            }

            return result;
        }


    }
}
