﻿using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class ScoreGoal
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;

        public ScoreGoal(Guid gameId, TeamType team, int playerNumber)
        {
            GameId = gameId;
            Team = team;
            PlayerNumber = playerNumber;
        }
    }
}
