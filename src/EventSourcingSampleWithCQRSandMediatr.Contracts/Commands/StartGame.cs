using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class StartGame : ICommand
    {
        public Guid GameId { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public StartGame(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
