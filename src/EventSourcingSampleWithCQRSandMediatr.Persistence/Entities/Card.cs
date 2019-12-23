using System;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Card
    {
        public Guid GameId { get; set; }
        public Guid TeamId { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime ShowedCartAt { get; set; } = DateTime.UtcNow;
    }
}
