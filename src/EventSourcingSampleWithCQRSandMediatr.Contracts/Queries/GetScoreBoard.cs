using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Queries;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Queries
{
    public class GetScoreBoard : IQuery<ScoreBoard>
    {
        public Guid GameId { get; set; }

        public GetScoreBoard(Guid gameId)
        {
            GameId = gameId;
        }
    }
}
