using EventSourcingSampleWithCQRSandMediatr.Domain.Events;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Events
{
    public class GameHasStarted : IEvent
    {
        public Guid GameId { get; set; }
        public GameHasStarted(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
