using EventSourcingSampleWithCQRSandMediatr.Domain.Events;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Events
{
    public class GameHasCreated : IEvent
    {
        public Guid GameId { get; set; }
        public GameHasCreated(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
