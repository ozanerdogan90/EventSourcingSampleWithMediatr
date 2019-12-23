using EventSourcingSampleWithCQRSandMediatr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.DataAccess.Repositories
{
    public interface IAuditRepository
    {
        Task<List<Audit>> GetById(Guid id);
        Task<List<Audit>> GetByTable(string name);
    }

    public class AuditRepository : IAuditRepository
    {
        private readonly Context context;
        public AuditRepository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<List<Audit>> GetById(Guid id)
        {
            return context.Audits.AsNoTracking().Where(x => x.PrimaryKey == id).ToListAsync();
        }

        public Task<List<Audit>> GetByTable(string name)
        {
            return context.Audits.AsNoTracking().Where(x => x.TableName == name).ToListAsync();
        }
    }
}
