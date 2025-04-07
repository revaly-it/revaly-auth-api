using MediatR;
using revaly.auth.Application.Models;

namespace revaly.auth.Application.Commands.UserCommand.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<ResultViewModel<UserViewModel>>
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
