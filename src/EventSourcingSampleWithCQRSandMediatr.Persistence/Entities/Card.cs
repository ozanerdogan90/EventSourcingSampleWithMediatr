using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Card
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime ShowedCartAt { get; set; } = DateTime.UtcNow;
    }
}
