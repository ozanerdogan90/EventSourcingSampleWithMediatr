using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Events
{

    public class PlayerHasScored
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }

        public PlayerHasScored(Guid gameId, TeamType team, int playerNumber)
        {
            GameId = gameId;
            Team = team;
            PlayerNumber = playerNumber;
        }
    }
}
