using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class EndGame : ICommand
    {
        public Guid GameId { get; set; }
        public DateTime EndedAt { get; set; } = DateTime.UtcNow;

        public EndGame(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
