using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Events;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Events
{
    public class RefereeHasShowedCard : IEvent
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public RefereeHasShowedCard(Guid gameId, TeamType teamType, int playerNumber)
        {
            GameId = gameId;
            Team = teamType;
            PlayerNumber = playerNumber;
        }
    }
}
