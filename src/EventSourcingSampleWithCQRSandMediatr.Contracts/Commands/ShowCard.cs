using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class ShowCard : ICommand
    {
        public Guid GameId { get; set; }
        public TeamType Team { get; set; }
        public int PlayerNumber { get; set; }
        public DateTime CardAt { get; set; } = DateTime.UtcNow;
        public ShowCard()
        {

        }
        public ShowCard(Guid gameId, TeamType teamType, int playerNumber)
        {
            GameId = gameId;
            Team = teamType;
            PlayerNumber = playerNumber;
        }
    }

    public class ShowCardValidator : AbstractValidator<ShowCard>
    {
        public ShowCardValidator()
        {
            RuleFor(x => x.GameId).NotEmpty();
            RuleFor(x => x.PlayerNumber).NotEmpty();
        }
    }
}
