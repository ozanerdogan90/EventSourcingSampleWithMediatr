using System;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Faul
    {
        public Guid GameId { get; set; }
        public Guid TeamId { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime FauledAt { get; set; } = DateTime.UtcNow;
    }
}
