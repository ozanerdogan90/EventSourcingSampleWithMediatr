using EventSourcingSampleWithCQRSandMediatr.Contracts.Commands;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Queries;
using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using EventSourcingSampleWithCQRSandMediatr.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Controllers
{
    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;
        public GameController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([BindRequired, FromBody]CreateGame command)
        {
            await this.commandBus.Send(command);
            return Created("games", command.Id);
        }

        [HttpPut]
        [Route("{id}/start")]
        public async Task<IActionResult> StartGame([BindRequired, FromQuery]Guid id, [BindRequired, FromBody]StartGame command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/end")]
        public async Task<IActionResult> EndGame([BindRequired, FromQuery]Guid id, [BindRequired, FromBody]EndGame command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/statistics/faul")]
        public async Task<IActionResult> AddFaul([BindRequired, FromQuery]Guid id, [BindRequired, FromBody]Faul command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/statistics/card")]
        public async Task<IActionResult> AddCard([BindRequired, FromQuery]Guid id, [BindRequired, FromBody]ShowCard command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("{id}/statistics/score")]
        public async Task<IActionResult> AddScore([BindRequired, FromQuery]Guid id, [BindRequired, FromBody]ScoreGoal command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpGet]
        [Route("{id}/score-board")]
        public async Task<IActionResult> GetScoreBoard([BindRequired, FromQuery]Guid id)
        {
            var results = await this.queryBus.Send<GetScoreBoard, ScoreBoard>(new GetScoreBoard(id));
            if (results == default)
                return NotFound();

            return Ok(results);
        }

        [HttpGet]
        [Route("{id}/details")]
        public async Task<IActionResult> GetStatistics([BindRequired, FromQuery]Guid id)
        {
            var results = await this.queryBus.Send<GetDetailedGame, GameDetails>(new GetDetailedGame(id));
            if (results == default)
                return NotFound();

            return Ok(results);
        }
    }
}
