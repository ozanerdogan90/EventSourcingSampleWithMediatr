using AutoFixture;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Commands;
using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingSampleWithCQRSandMediatr.Tests.Controllers
{
    public class GameStatisticsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly Fixture fixture;
        public GameStatisticsControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            this.client = factory.CreateClient();
            this.fixture = new Fixture();
        }

        private Task<HttpResponseMessage> CreateGame()
        {
            var game = fixture.Create<CreateGame>();
            game.HomeTeam.Players = fixture.CreateMany<Player>(18).ToList();
            game.AwayTeam.Players = fixture.CreateMany<Player>(18).ToList();
            return client.PostAsync("/games", game.ToContent());
        }

        [Fact]
        public async Task Faul_EmptyGuid_ReturnsBadRequest()
        {
            var command = fixture.Create<Faul>();
            var response = await client.PutAsync($"/games/{Guid.Empty}/statistics/faul", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Faul_EmptyPayload_ReturnsBadRequest()
        {
            var command = new Faul();
            var response = await client.PutAsync($"/games/{Guid.NewGuid()}/statistics/faul", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Faul_NonExistingGame_ReturnsInternalServerError()
        {
            var command = fixture.Create<Faul>();
            command.GameId = Guid.NewGuid();
            var response = await client.PutAsync($"/games/{command.GameId}/statistics/faul", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Faul_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();

            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var command = fixture.Create<Faul>();
            command.GameId = gameId;
            var response = await client.PutAsync($"/games/{gameId}/statistics/faul", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddCard_EmptyGuid_ReturnsBadRequest()
        {
            var command = fixture.Create<ShowCard>();
            var response = await client.PutAsync($"/games/{Guid.Empty}/statistics/card", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddCard_EmptyPayload_ReturnsBadRequest()
        {
            var command = new ShowCard();
            var response = await client.PutAsync($"/games/{Guid.NewGuid()}/statistics/card", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddCard_NonExistingGame_ReturnsInternalServerError()
        {
            var command = fixture.Create<ShowCard>();
            command.GameId = Guid.NewGuid();
            var response = await client.PutAsync($"/games/{command.GameId}/statistics/card", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddCard_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();

            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var command = fixture.Create<ShowCard>();
            command.GameId = gameId;
            var response = await client.PutAsync($"/games/{gameId}/statistics/card", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [Fact]
        public async Task ScoreGoal_EmptyGuid_ReturnsBadRequest()
        {
            var command = fixture.Create<ScoreGoal>();
            var response = await client.PutAsync($"/games/{Guid.Empty}/statistics/score", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ScoreGoal_EmptyPayload_ReturnsBadRequest()
        {
            var command = new ScoreGoal();
            var response = await client.PutAsync($"/games/{Guid.NewGuid()}/statistics/score", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ScoreGoal_NonExistingGame_ReturnsInternalServerError()
        {
            var command = fixture.Create<ScoreGoal>();
            command.GameId = Guid.NewGuid();
            var response = await client.PutAsync($"/games/{command.GameId}/statistics/score", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ScoreGoal_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();

            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var command = fixture.Create<ScoreGoal>();
            command.GameId = gameId;
            var response = await client.PutAsync($"/games/{gameId}/statistics/score", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetGameDetails_EmptyGuid_ReturnsBadRequest()
        {
            var response = await client.GetAsync($"/games/{Guid.Empty}/statistics");
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetGameDetails_NonExistingGame_ReturnsNotFound()
        {
            var response = await client.GetAsync($"/games/{Guid.NewGuid()}/statistics");
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetGameDetails_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();
            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var response = await client.GetAsync($"/games/{gameId}/statistics");
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
