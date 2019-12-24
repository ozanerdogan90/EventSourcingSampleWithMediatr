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
    public class GameCommandHandler :
        ICommandHandler<CreateGame>,
        ICommandHandler<EndGame>,
        ICommandHandler<StartGame>
    {
        private readonly IEventBus eventBus;
        private readonly IGameRepository gameRepository;
        public GameCommandHandler(IGameRepository gameRepository, IEventBus eventBus)
        {
            this.eventBus = eventBus;
            this.gameRepository = gameRepository;
        }

        public async Task<Unit> Handle(EndGame request, CancellationToken cancellationToken)
        {
            var isSuccesful = await this.gameRepository.EndGame(request.GameId);
            if (!isSuccesful)
                throw new Exception($"Game with id{request.GameId} couldnt be ended");

            await eventBus.Publish(new GameHasEnded(request.GameId));
            return Unit.Value;
        }

        public async Task<Unit> Handle(CreateGame request, CancellationToken cancellationToken)
        {
            var game = new Persistence.Entities.Game()
            {
                AwayTeamId = request.AwayTeam.Id,
                HomeTeamId = request.HomeTeam.Id,
                StadiumName = request.StadiumName,
                Id = request.Id,
            };

            var isSuccesful = await this.gameRepository.CreateGame(game);
            if (!isSuccesful)
                throw new Exception($"Game with id{request.Id} couldnt be created");

            await eventBus.Publish(new GameHasCreated(request.Id));
            return Unit.Value;
        }

        public async Task<Unit> Handle(StartGame request, CancellationToken cancellationToken)
        {
            var isSuccesful = await this.gameRepository.StartGame(request.GameId);
            if (!isSuccesful)
                throw new Exception($"Game with id{request.GameId} couldnt be started");

            await eventBus.Publish(new GameHasStarted(request.GameId));
            return Unit.Value;
        }
    }
}
