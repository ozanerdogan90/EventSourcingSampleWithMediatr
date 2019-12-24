using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
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
}
