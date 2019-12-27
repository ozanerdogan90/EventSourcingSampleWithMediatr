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
    public class GameControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly Fixture fixture;
        public GameControllerIntegrationTests(WebApplicationFactory<Startup> factory)
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
        public async Task CreateGame_InvalidPayload_ReturnsBadRequest()
        {
            var game = fixture.Create<CreateGame>();
            var response = await client.PostAsync("/games", game.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateGame_Success_ReturnsCreated()
        {
            var response = await CreateGame();

            response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }


        [Fact]
        public async Task Start_EmptyGuid_ReturnsBadRequest()
        {
            var command = fixture.Create<StartGame>();
            var response = await client.PutAsync($"/games/{Guid.Empty}/start", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Start_EmptyPayload_ReturnsBadRequest()
        {
            var command = new StartGame();
            var response = await client.PutAsync($"/games/{Guid.NewGuid()}/start", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Start_NonExistingGame_ReturnsInternalServerError()
        {
            var command = fixture.Create<StartGame>();
            command.GameId = Guid.NewGuid();
            var response = await client.PutAsync($"/games/{command.GameId}/start", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Start_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();

            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var command = fixture.Create<StartGame>();
            command.GameId = gameId;
            var response = await client.PutAsync($"/games/{gameId}/start", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [Fact]
        public async Task End_EmptyGuid_ReturnsBadRequest()
        {
            var command = fixture.Create<EndGame>();
            var response = await client.PutAsync($"/games/{Guid.Empty}/end", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task End_EmptyPayload_ReturnsBadRequest()
        {
            var command = new EndGame();
            var response = await client.PutAsync($"/games/{Guid.NewGuid()}/end", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task End_NonExistingGame_ReturnsInternalServerError()
        {
            var command = fixture.Create<EndGame>();
            command.GameId = Guid.NewGuid();
            var response = await client.PutAsync($"/games/{command.GameId}/end", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task End_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();

            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var command = fixture.Create<EndGame>();
            command.GameId = gameId;
            var response = await client.PutAsync($"/games/{gameId}/end", command.ToContent());

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetScoreBoard_EmptyGuid_ReturnsBadRequest()
        {
            var response = await client.GetAsync($"/games/{Guid.Empty}/score-board");
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetScoreBoard_NonExistingGame_ReturnsNotFound()
        {
            var response = await client.GetAsync($"/games/{Guid.NewGuid()}/score-board");
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetScoreBoard_Success_ReturnsOk()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();
            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var response = await client.GetAsync($"/games/{gameId}/score-board");
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
