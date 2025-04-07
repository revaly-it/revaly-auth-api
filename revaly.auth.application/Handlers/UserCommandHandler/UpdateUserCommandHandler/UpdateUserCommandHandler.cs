using MediatR;
using revaly.auth.Application.Commands.UserCommand.UpdateUserCommand;
using revaly.auth.Application.Models;
using revaly.auth.Domain.Entities;
using revaly.auth.Domain.Entities.Enums;
using revaly.auth.Domain.Interfaces.IUnitOfWork;

namespace revaly.auth.Application.Handlers.UserCommandHandler.UpdateUserCommandHandler
{
    public class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, ResultViewModel<UserViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultViewModel<UserViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.GetUserByIdAsync(request.Id);

            if (user == null)
            {
                return ResultViewModel<UserViewModel>.Error($"User with ID {request.Id} not found.");
            }

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _unitOfWork.User.UpdateUserAsync(user);
            await _unitOfWork.CompleteAsync();

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.UtcNow.AddHours(-3)
            };

            return ResultViewModel<UserViewModel>.Success(viewModel, "User updated successfully.");
        }
    }

}
