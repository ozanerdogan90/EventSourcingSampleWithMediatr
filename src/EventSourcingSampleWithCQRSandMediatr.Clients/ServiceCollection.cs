using EventSourcingSampleWithCQRSandMediatr.Clients.Handlers;
using EventSourcingSampleWithCQRSandMediatr.Clients.Queries;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Commands;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Queries;
using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using EventSourcingSampleWithCQRSandMediatr.Domain.Events;
using EventSourcingSampleWithCQRSandMediatr.Domain.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingSampleWithCQRSandMediatr.Clients
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddCQRSServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<IEventBus, EventBus>();

            services.AddScoped<IRequestHandler<StartGame, Unit>, GameCommandHandler>();
            services.AddScoped<IRequestHandler<EndGame, Unit>, GameCommandHandler>();
            services.AddScoped<IRequestHandler<CreateGame, Unit>, GameCommandHandler>();

            services.AddScoped<IRequestHandler<ScoreGoal, Unit>, StatisticCommandHandler>();
            services.AddScoped<IRequestHandler<Faul, Unit>, StatisticCommandHandler>();
            services.AddScoped<IRequestHandler<ShowCard, Unit>, StatisticCommandHandler>();

            services.AddScoped<IRequestHandler<GetScoreBoard, ScoreBoard>, GameQueryHandler>();
            services.AddScoped<IRequestHandler<GetDetailedGame, GameDetails>, GameQueryHandler>();

            return services;
        }
    }
}
