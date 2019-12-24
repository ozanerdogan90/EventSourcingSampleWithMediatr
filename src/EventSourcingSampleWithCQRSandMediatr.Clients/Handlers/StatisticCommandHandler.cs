using EventSourcingSampleWithCQRSandMediatr.Contracts.Commands;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Events;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using EventSourcingSampleWithCQRSandMediatr.Domain.Events;
using EventSourcingSampleWithCQRSandMediatr.Persistence.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Clients.Handlers
{
    public class StatisticCommandHandler :
        ICommandHandler<ShowCard>,
        ICommandHandler<Faul>,
        ICommandHandler<ScoreGoal>
    {
        private readonly IEventBus eventBus;
        private readonly IGameRepository gameRepository;
        public StatisticCommandHandler(IGameRepository gameRepository, IEventBus eventBus)
        {
            this.eventBus = eventBus;
            this.gameRepository = gameRepository;
        }
        private async Task ValidateGame(Guid gameId)
        {
            var doesGameExist = await this.gameRepository.DoesGameExist(gameId);
            if (!doesGameExist)
                throw new Exception($"Game with id:{gameId} doesnt exist");
        }

        public async Task<Unit> Handle(ShowCard request, CancellationToken cancellationToken)
        {
            await ValidateGame(request.GameId);

            var isSuccesful = await gameRepository.AddCard(new Persistence.Entities.Card() { GameId = request.GameId, PlayerNumber = request.PlayerNumber, Team = request.Team });
            if (!isSuccesful)
                throw new Exception($"Card with gameid :{request.GameId} couldnt be added");

            await this.eventBus.Publish(new RefereeHasShowedCard(request.GameId, request.Team, request.PlayerNumber));
            return Unit.Value;
        }

        public async Task<Unit> Handle(Faul request, CancellationToken cancellationToken)
        {
            await ValidateGame(request.GameId);

            var isSuccesful = await gameRepository.AddFaul(new Persistence.Entities.Faul() { GameId = request.GameId, PlayerNumber = request.PlayerNumber, Team = request.Team });
            if (!isSuccesful)
                throw new Exception($"Faul with gameid :{request.GameId} couldnt be added");

            await this.eventBus.Publish(new PlayerHasFauled(request.GameId, request.Team, request.PlayerNumber));
            return Unit.Value;
        }

        public async Task<Unit> Handle(ScoreGoal request, CancellationToken cancellationToken)
        {
            await ValidateGame(request.GameId);

            var isSuccesful = await gameRepository.AddScore(new Persistence.Entities.Score() { GameId = request.GameId, PlayerNumber = request.PlayerNumber, Team = request.Team });
            if (!isSuccesful)
                throw new Exception($"Score with gameid :{request.GameId} couldnt be added");

            await this.eventBus.Publish(new PlayerHasFauled(request.GameId, request.Team, request.PlayerNumber));
            return Unit.Value;
        }
    }
}
