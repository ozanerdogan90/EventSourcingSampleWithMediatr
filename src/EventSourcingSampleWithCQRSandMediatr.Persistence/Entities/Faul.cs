using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Faul
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime FauledAt { get; set; } = DateTime.UtcNow;
    }
}
