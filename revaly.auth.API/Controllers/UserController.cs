using MediatR;
using Microsoft.AspNetCore.Mvc;
using revaly.auth.Application.Commands.AuthCommand.RegisterUserCommand;

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
        /// Register user 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }


    }
}
