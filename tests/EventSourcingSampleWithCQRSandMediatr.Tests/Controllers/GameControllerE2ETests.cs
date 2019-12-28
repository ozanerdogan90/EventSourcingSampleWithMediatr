using AutoFixture;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Commands;
using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingSampleWithCQRSandMediatr.Tests.Controllers
{
    public class GameControllerE2ETests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly Fixture fixture;
        private CreateGame game;
        public GameControllerE2ETests(WebApplicationFactory<Startup> factory)
        {
            this.client = factory.CreateClient();
            this.fixture = new Fixture();
        }
        public CreateGame Game
        {
            get
            {
                if (game != default)
                    return game;

                game = fixture.Create<CreateGame>();
                game.HomeTeam.Players = fixture.CreateMany<Player>(18).ToList();
                game.AwayTeam.Players = fixture.CreateMany<Player>(18).ToList();

                return game;
            }
        }


        private Task<HttpResponseMessage> CreateGame()
        {
            return client.PostAsync("/games", Game.ToContent());
        }

        private List<Faul> CreateRandomFauls()
        {
            var random = new Random();
            var result = new List<Faul>();

            result.AddRange(Enumerable.Range(0, random.Next(0, 20)).ToList().Select(index =>
             {
                 return new Faul
                 {
                     GameId = Game.Id,
                     PlayerNumber = Game.HomeTeam.Players[random.Next(1, 18)].JerseyNumber,
                     Team = TeamType.Home,
                     FaulAt = DateTime.UtcNow.AddMinutes(random.Next(0, 90))
                 };
             }));

            result.AddRange(Enumerable.Range(0, random.Next(0, 20)).ToList().Select(index =>
            {
                return new Faul
                {
                    GameId = Game.Id,
                    PlayerNumber = Game.AwayTeam.Players[random.Next(1, 18)].JerseyNumber,
                    Team = TeamType.Away,
                    FaulAt = DateTime.UtcNow.AddMinutes(random.Next(0, 90))
                };
            }));

            return result;
        }

        private List<ShowCard> CreateRandomCards()
        {
            var random = new Random();
            var result = new List<ShowCard>();

            result.AddRange(Enumerable.Range(0, random.Next(0, 5)).ToList().Select(index =>
            {
                return new ShowCard
                {
                    GameId = Game.Id,
                    PlayerNumber = Game.HomeTeam.Players[random.Next(1, 18)].JerseyNumber,
                    Team = TeamType.Home,
                    CardAt = DateTime.UtcNow.AddMinutes(random.Next(0, 90))
                };
            }));

            result.AddRange(Enumerable.Range(0, random.Next(0, 3)).ToList().Select(index =>
            {
                return new ShowCard
                {
                    GameId = Game.Id,
                    PlayerNumber = Game.AwayTeam.Players[random.Next(1, 18)].JerseyNumber,
                    Team = TeamType.Away,
                    CardAt = DateTime.UtcNow.AddMinutes(random.Next(0, 90))
                };
            }));

            return result;
        }

        private List<ScoreGoal> CreateRandomScores()
        {
            var random = new Random();
            var result = new List<ScoreGoal>();

            result.AddRange(Enumerable.Range(0, random.Next(0, 6)).ToList().Select(index =>
            {
                return new ScoreGoal
                {
                    GameId = Game.Id,
                    PlayerNumber = Game.HomeTeam.Players[random.Next(1, 18)].JerseyNumber,
                    Team = TeamType.Home,
                    ScoredAt = DateTime.UtcNow.AddMinutes(random.Next(0, 90))
                };
            }));

            result.AddRange(Enumerable.Range(0, random.Next(0, 6)).ToList().Select(index =>
             {
                 return new ScoreGoal
                 {
                     GameId = Game.Id,
                     PlayerNumber = Game.AwayTeam.Players[random.Next(1, 18)].JerseyNumber,
                     Team = TeamType.Away,
                     ScoredAt = DateTime.UtcNow.AddMinutes(random.Next(0, 90))
                 };
             }));

            return result;
        }

        private async Task StartGame(Guid id)
        {
            var command = new StartGame(id);
            command.StartedAt = DateTime.UtcNow;
            var response = await client.PutAsync($"/games/{command.GameId}/start", command.ToContent());
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        private async Task EndGame(Guid id)
        {
            var command = new EndGame(id);
            command.EndedAt = DateTime.UtcNow.AddMinutes(90);
            var response = await client.PutAsync($"/games/{command.GameId}/end", command.ToContent());
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        private async Task AddFaul(Faul command)
        {
            var response = await client.PutAsync($"/games/{command.GameId}/statistics/faul", command.ToContent());
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        private async Task ShowCard(ShowCard command)
        {
            var response = await client.PutAsync($"/games/{command.GameId}/statistics/card", command.ToContent());
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        private async Task ScoreGoal(ScoreGoal command)
        {
            var response = await client.PutAsync($"/games/{command.GameId}/statistics/score", command.ToContent());
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        private async Task<ScoreBoard> GetScoreBoard(Guid gameId)
        {
            var response = await client.GetAsync($"/games/{gameId}/score-board");
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            return JsonConvert.DeserializeObject<ScoreBoard>(await response.Content.ReadAsStringAsync());
        }

        private async Task<GameDetails> GetGameDetails(Guid gameId)
        {
            var response = await client.GetAsync($"/games/{gameId}/statistics");
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            return JsonConvert.DeserializeObject<GameDetails>(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task AllFlow_Success_ReturnsDetails()
        {
            var createGameResponse = await CreateGame();
            var gameIdString = await createGameResponse.Content.ReadAsStringAsync();
            var gameId = Guid.Parse(gameIdString.Replace("\"", string.Empty));
            var scores = CreateRandomScores();
            var fauls = CreateRandomFauls();
            var cards = CreateRandomCards();

            await StartGame(gameId);
            await Task.WhenAll(scores.Select(l => ScoreGoal(l))
                        .ToArray());

            await Task.WhenAll(fauls.Select(l => AddFaul(l))
                 .ToArray());

            await Task.WhenAll(cards.Select(l => ShowCard(l))
                 .ToArray());

            await EndGame(gameId);

            var scoreBoard = await GetScoreBoard(gameId);
            var gameDetails = await GetGameDetails(gameId);

            scoreBoard.Should().NotBeNull();
            scoreBoard.GameId.Should().Be(gameId);
            scoreBoard.HomeScore.Should().Be(scores.Where(x => x.Team == TeamType.Home).Count());
            scoreBoard.AwayScore.Should().Be(scores.Where(x => x.Team == TeamType.Away).Count());

            gameDetails.HomeScore.Should().Be(scores.Where(x => x.Team == TeamType.Home).Count());
            gameDetails.AwayScore.Should().Be(scores.Where(x => x.Team == TeamType.Away).Count());

            gameDetails.Statistics.Where(x => x.Team == TeamType.Home && x.Type.Equals("faul", StringComparison.InvariantCultureIgnoreCase)).Count()
                .Should().Be(fauls.Count(y => y.Team == TeamType.Home));

            gameDetails.Statistics.Where(x => x.Team == TeamType.Away && x.Type.Equals("faul", StringComparison.InvariantCultureIgnoreCase)).Count()
             .Should().Be(fauls.Count(y => y.Team == TeamType.Away));

            gameDetails.Statistics.Where(x => x.Team == TeamType.Home && x.Type.Equals("score", StringComparison.InvariantCultureIgnoreCase)).Count()
        .Should().Be(scores.Count(y => y.Team == TeamType.Home));

            gameDetails.Statistics.Where(x => x.Team == TeamType.Away && x.Type.Equals("score", StringComparison.InvariantCultureIgnoreCase)).Count()
             .Should().Be(scores.Count(y => y.Team == TeamType.Away));

            gameDetails.Statistics.Where(x => x.Team == TeamType.Home && x.Type.Equals("card", StringComparison.InvariantCultureIgnoreCase)).Count()
    .Should().Be(cards.Count(y => y.Team == TeamType.Home));

            gameDetails.Statistics.Where(x => x.Team == TeamType.Away && x.Type.Equals("card", StringComparison.InvariantCultureIgnoreCase)).Count()
             .Should().Be(cards.Count(y => y.Team == TeamType.Away));
        }

    }
}
