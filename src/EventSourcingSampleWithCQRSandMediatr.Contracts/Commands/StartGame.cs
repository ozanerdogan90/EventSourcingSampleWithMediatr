using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using FluentValidation;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class StartGame : ICommand
    {
        public Guid GameId { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        public StartGame()
        {

        }
        public StartGame(Guid gameId)
        {
            GameId = gameId;
        }
    }

    public class StartGameValidator : AbstractValidator<StartGame>
    {
        public StartGameValidator()
        {
            RuleFor(x => x.GameId).NotEmpty();
        }
    }
}
