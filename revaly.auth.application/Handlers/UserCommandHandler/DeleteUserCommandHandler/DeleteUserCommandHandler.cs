using MediatR;
using revaly.auth.Application.Commands.UserCommand.DeleteUserCommand;
using revaly.auth.Application.Models;
using revaly.auth.Domain.Interfaces.IUnitOfWork;

namespace revaly.auth.Application.Handlers.UserCommandHandler.DeleteUserCommandHandler
{
    public class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, ResultViewModel<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultViewModel<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.UserExistsAsync(request.Id);

            if (!user)
            {
                return ResultViewModel<bool>.Error("User not found");
            }

            await _unitOfWork.User.DeleteUserAsync(request.Id);
            await _unitOfWork.CompleteAsync();

            return ResultViewModel<bool>.Success(true);
        }
    }
}
