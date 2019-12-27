using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using FluentValidation;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class ScoreGoal : ICommand
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;
        public ScoreGoal()
        {

        }

        public ScoreGoal(Guid gameId, TeamType team, int playerNumber)
        {
            GameId = gameId;
            Team = team;
            PlayerNumber = playerNumber;
        }
    }

    public class ScoreGoalValidator : AbstractValidator<ScoreGoal>
    {
        public ScoreGoalValidator()
        {
            RuleFor(x => x.GameId).Must(y => y != null && y != Guid.Empty);
            RuleFor(x => x.PlayerNumber).NotEmpty();
        }
    }
}
