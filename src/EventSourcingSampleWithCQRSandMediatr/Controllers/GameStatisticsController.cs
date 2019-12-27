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
    [ApiController]
    [Route("games/{id}/statistics")]
    public class GameStatisticsController : ControllerBase
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryBus queryBus;
        public GameStatisticsController(ICommandBus commandBus, IQueryBus queryBus)
        {
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
            this.queryBus = queryBus ?? throw new ArgumentNullException(nameof(queryBus));
        }

        [HttpPut]
        [Route("faul")]
        public async Task<IActionResult> AddFaul([NotEmptyGuid, FromRoute]Guid id, [BindRequired, FromBody]Faul command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("card")]
        public async Task<IActionResult> ShowCard([NotEmptyGuid, FromRoute]Guid id, [BindRequired, FromBody]ShowCard command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpPut]
        [Route("score")]
        public async Task<IActionResult> ScoreGoal([NotEmptyGuid, FromRoute]Guid id, [BindRequired, FromBody]ScoreGoal command)
        {
            command.GameId = id;
            await this.commandBus.Send(command);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetGameDetails([NotEmptyGuid, FromRoute]Guid id)
        {
            var results = await this.queryBus.Send<GetDetailedGame, GameDetails>(new GetDetailedGame(id));
            if (results == default)
                return NotFound();

            return Ok(results);
        }
    }
}
