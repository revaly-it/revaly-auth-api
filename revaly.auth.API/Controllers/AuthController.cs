using MediatR;
using Microsoft.AspNetCore.Mvc;
using revaly.auth.Application.Commands.AuthCommand.RegisterUserCommand;
using revaly.auth.Application.Queries.LoginQuery;

namespace revaly.auth.API.Controllers
{
    /// <summary>
    /// Login Controller
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery request)
        {
            var result = await mediator.Send(request);

            if(!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var result = await mediator.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
