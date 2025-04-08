using MediatR;
using revaly.auth.Application.Models;

namespace revaly.auth.Application.Commands.UserCommand.DeleteUserCommand
{
    public class DeleteUserCommand : IRequest<ResultViewModel<bool>>
    {
        public Guid Id { get; set; }
    }
}
