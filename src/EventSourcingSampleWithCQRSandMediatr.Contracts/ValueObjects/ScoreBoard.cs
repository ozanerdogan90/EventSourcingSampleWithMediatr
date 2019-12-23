using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects
{
    public class ScoreBoard
    {
        public Guid GameId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public ScoreBoard(Guid gameId, int homeScore, int awayScore)
        {
            GameId = gameId;
            HomeScore = homeScore;
            AwayScore = awayScore;
        }
    }
}