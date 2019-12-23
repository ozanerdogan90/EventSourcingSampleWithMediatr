using System;

namespace EventSourcingSampleWithCQRSandMediatr.DataAccess.Entities
{

    public class Audit : BaseEntity
    {
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public Guid PrimaryKey { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string State { get; set; }
    }

}
