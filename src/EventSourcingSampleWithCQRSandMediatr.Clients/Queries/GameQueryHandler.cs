using EventSourcingSampleWithCQRSandMediatr.Contracts.Queries;
using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Queries;
using EventSourcingSampleWithCQRSandMediatr.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Clients.Queries
{
    public class GameQueryHandler :
            IQueryHandler<GetScoreBoard, ScoreBoard>,
            IQueryHandler<GetDetailedGame, GameDetails>
    {
        private readonly IGameRepository gameRepository;
        public GameQueryHandler(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
        }

        private async Task ValidateGame(Guid gameId)
        {
            var doesGameExist = await this.gameRepository.DoesGameExist(gameId);
            if (!doesGameExist)
                throw new Exception($"Game with id:{gameId} doesnt exist");
        }

        public async Task<GameDetails> Handle(GetDetailedGame request, CancellationToken cancellationToken)
        {
            await ValidateGame(request.GameId);
            var scores = await this.gameRepository.GetScores(request.GameId);
            var faules = await this.gameRepository.GetFauls(request.GameId);
            var cards = await this.gameRepository.GetCards(request.GameId);

            var statistics = new List<Statistics>();
            statistics.AddRange(faules.Select(x => new Statistics() { Type = x.GetType().Name, Team = x.Team, ActionBy = x.PlayerNumber.ToString() }));
            statistics.AddRange(scores.Select(x => new Statistics() { Type = x.GetType().Name, Team = x.Team, ActionBy = x.PlayerNumber.ToString() }));
            statistics.AddRange(faules.Select(x => new Statistics() { Type = x.GetType().Name, Team = x.Team, ActionBy = x.PlayerNumber.ToString() }));

            return new GameDetails()
            {
                HomeScore = scores.Where(x => x.Team == TeamType.Home).Count(),
                AwayScore = scores.Where(x => x.Team == TeamType.Away).Count(),
                GameId = request.GameId,
                Statistics = statistics
            };

        }

        public async Task<ScoreBoard> Handle(GetScoreBoard request, CancellationToken cancellationToken)
        {
            await ValidateGame(request.GameId);
            var scores = await this.gameRepository.GetScores(request.GameId);
            var homeScores = scores.Where(x => x.Team == TeamType.Home).Count();
            var awayScores = scores.Where(x => x.Team == TeamType.Away).Count();
            return new ScoreBoard(request.GameId, homeScores, awayScores);
        }
    }
}
