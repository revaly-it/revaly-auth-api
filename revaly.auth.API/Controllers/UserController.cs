using MediatR;
using Microsoft.AspNetCore.Mvc;
using revaly.auth.Application.Commands.UserCommand.UpdateUserCommand;

namespace revaly.auth.API.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
