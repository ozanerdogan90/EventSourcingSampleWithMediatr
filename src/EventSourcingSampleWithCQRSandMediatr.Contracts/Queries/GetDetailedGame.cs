using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Queries;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Queries
{
    public class GetDetailedGame : IQuery<GameDetails>
    {
        public Guid GameId { get; set; }
        public GetDetailedGame(Guid gameId)
        {
            this.GameId = gameId;
        }
    }
}
