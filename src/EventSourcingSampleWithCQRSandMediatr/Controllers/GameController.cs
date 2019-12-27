using EventSourcingSampleWithCQRSandMediatr.Contracts.Commands;
using EventSourcingSampleWithCQRSandMediatr.Contracts.Queries;
using EventSourcingSampleWithCQRSandMediatr.Contracts.ValueObjects;
using EventSourcingSampleWithCQRSandMediatr.Domain.Commands;
using EventSourcingSampleWithCQRSandMediatr.Domain.Queries;
using EventSourcingSampleWithCQRSandMediatr.Models.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Controllers
{
    /// <summary>
    /// Games Controller
    /// </summary>
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

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <param name="command">Input</param>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">Invalid payload</response>
        /// <response code="500">Something went wrong</response>
        [HttpPost]
        public async Task<IActionResult> CreateGame([BindRequired, FromBody]CreateGame command)
        {
            await this.commandBus.Send(command);
            return Created("games", command.Id);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="id">Game id</param>
        /// <param name="command">Input</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid payload</response>
        /// <response code="500">Something went wrong</response>
        [HttpPut]
        [Route("{id}/start")]
        public async Task<IActionResult> StartGame([NotEmptyGuid, FromRoute]Guid id, [BindRequired, FromBody]StartGame command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <param name="id">Game id</param>
        /// <param name="command">Input</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid payload</response>
        /// <response code="500">Something went wrong</response>
        [HttpPut]
        [Route("{id}/end")]
        public async Task<IActionResult> EndGame([NotEmptyGuid, FromRoute]Guid id, [BindRequired, FromBody]EndGame command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get the game score-board.
        /// </summary>
        /// <param name="id">Game id</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Game doesnt exist</response>
        /// <response code="500">Something went wrong</response>
        [HttpGet]
        [Route("{id}/score-board")]
        public async Task<IActionResult> GetScoreBoard([NotEmptyGuid, FromRoute]Guid id)
        {
            var results = await this.queryBus.Send<GetScoreBoard, ScoreBoard>(new GetScoreBoard(id));
            if (results == default)
                return NotFound();

            return Ok(results);
        }

    }
}
