using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcingSampleWithCQRSandMediatr.DataAccess.Entities
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public List<string> KeyValues { get; } = new List<string>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public Audit ToAudit()
        {
            var audit = new Audit();
            FillChangedValues();
            audit.State = Entry.State.ToString();
            audit.TableName = Entry.Entity.GetType().Name;
            audit.DateTime = DateTime.UtcNow;
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            if (Entry.Entity is BaseEntity baseEntity)
            {
                audit.PrimaryKey = baseEntity.Id;
            }
            return audit;
        }

        private void FillChangedValues()
        {
            if (this.Entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                foreach (var property in Entry.CurrentValues.Properties)
                {
                    if (Entry.CurrentValues[property.Name] != default)
                        NewValues.Add(property.Name, Entry.CurrentValues[property.Name]);
                }
            }
            else
            {
                foreach (var field in Entry.CurrentValues.Properties)
                {
                    var name = field.Name;

                    if (!Entry.OriginalValues[name].Equals(Entry.CurrentValues[name]))
                    {
                        KeyValues.Add(name);
                        OldValues.Add(name, Entry.OriginalValues[name]);
                    }
                }
            }

        }
    }
}