using System;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Entities
{
    public class Score
    {
        public Guid GameId { get; set; }
        public Guid TeamId { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;
    }
}
