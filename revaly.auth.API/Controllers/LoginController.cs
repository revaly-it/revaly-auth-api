using MediatR;
using Microsoft.AspNetCore.Mvc;
using revaly.auth.Application.Commands.LoginCommand;

namespace revaly.auth.API.Controllers
{

    /// <summary>
    /// Auth Controller
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class LoginController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
