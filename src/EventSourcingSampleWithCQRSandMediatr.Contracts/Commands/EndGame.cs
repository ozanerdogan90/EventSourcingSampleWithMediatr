using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using FluentValidation;
using System;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class EndGame : ICommand
    {
        public Guid GameId { get; set; }
        public DateTime EndedAt { get; set; } = DateTime.UtcNow;
        public EndGame()
        {

        }

        public EndGame(Guid gameId)
        {
            GameId = gameId;
        }
    }

    public class EndGameValidator : AbstractValidator<EndGame>
    {
        public EndGameValidator()
        {
            RuleFor(x => x.GameId).Must(y => y != null && y != Guid.Empty);
        }
    }
}
