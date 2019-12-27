using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace EventSourcingSampleWithCQRSandMediatr.Contracts.Commands
{
    public class CreateGame : ICommand
    {
        public Guid Id { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public string StadiumName { get; set; }
        public List<Referee> Referees { get; set; }
        public CreateGame()
        {

        }

        public CreateGame(Guid id, Team homeTeam, Team awayTeam, List<Referee> referees)
        {
            Id = id;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Referees = referees;
        }
    }

    public class CreateGameValidator : AbstractValidator<CreateGame>
    {
        public CreateGameValidator()
        {
            #region home team validaton
            RuleFor(m => m.HomeTeam).NotEmpty();
            RuleFor(m => m.HomeTeam.CoachName).NotEmpty();
            RuleFor(m => m.HomeTeam.Name).NotEmpty();
            RuleFor(m => m.HomeTeam.Players).NotEmpty();
            RuleFor(m => m.HomeTeam.Players).Must(y => y.Count == 18);
            RuleFor(m => m.HomeTeam.Players).Must(y => y.Exists(t => t.Position == Positions.Goolkeeper));
            RuleFor(m => m.HomeTeam.Players).Must(y => y.Exists(t => t.Position == Positions.Defender));
            RuleFor(m => m.HomeTeam.Players).Must(y => y.Exists(t => t.Position == Positions.Midfielder));
            #endregion

            #region
            RuleFor(m => m.AwayTeam).NotEmpty();
            RuleFor(m => m.AwayTeam.CoachName).NotEmpty();
            RuleFor(m => m.AwayTeam.Name).NotEmpty();
            RuleFor(m => m.AwayTeam.Players).NotEmpty();
            RuleFor(m => m.AwayTeam.Players).Must(y => y.Count == 18);
            RuleFor(m => m.AwayTeam.Players).Must(y => y.Exists(t => t.Position == Positions.Goolkeeper));
            RuleFor(m => m.AwayTeam.Players).Must(y => y.Exists(t => t.Position == Positions.Defender));
            RuleFor(m => m.AwayTeam.Players).Must(y => y.Exists(t => t.Position == Positions.Midfielder));
            #endregion

            RuleFor(m => m.StadiumName).NotEmpty();
            RuleFor(m => m.Referees).NotEmpty();
        }
    }
}
