using EventSourcingSampleWithCQRSandMediatr.Domain.Events;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Events
{
    public class GameHasEnded : IEvent
    {
        public Guid GameId { get; set; }
        public GameHasEnded(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
