using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class ShowCard
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime FaulAt { get; set; } = DateTime.UtcNow;
        public ShowCard(Guid gameId, TeamType teamType, int playerNumber)
        {
            GameId = gameId;
            Team = teamType;
            PlayerNumber = playerNumber;
        }
    }
}
