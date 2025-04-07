using MediatR;
using revaly.auth.Application.Models;

namespace revaly.auth.Application.Commands.AuthCommand.RegisterUserCommand
{
    public class RegisterUserCommand : IRequest<ResultViewModel<UserViewModel>>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
