using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class Faul : ICommand
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime FaulAt { get; set; } = DateTime.UtcNow;
        public Faul()
        {

        }
        public Faul(Guid gameId, TeamType teamType, int playerNumber)
        {
            GameId = gameId;
            Team = teamType;
            PlayerNumber = playerNumber;
        }
    }
}
