using MediatR;
using revaly.auth.Application.Models;

namespace revaly.auth.Application.Commands.LoginCommand
{
    public class LoginCommand : IRequest<ResultViewModel<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
