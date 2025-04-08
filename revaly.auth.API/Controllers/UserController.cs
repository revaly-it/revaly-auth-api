using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revaly.auth.Application.Commands.UserCommand.DeleteUserCommand;
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
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteUserCommand { Id = id });
            return Ok(result);
        }
    }
}
