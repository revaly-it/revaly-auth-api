
using MediatR;
using revaly.auth.Application.Commands.AuthCommand.RegisterUserCommand;
using revaly.auth.Application.Models;
using revaly.auth.Domain.Entities;
using revaly.auth.Domain.Entities.Enums;
using revaly.auth.Domain.Interfaces.IUnitOfWork;

namespace revaly.auth.Application.Handlers.AuthCommandHandler.RegisterUserCommandHandler
{
    public class RegisterUserCommandHandler (IUnitOfWork unitOfWork)
        : IRequestHandler<RegisterUserCommand, ResultViewModel<UserViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultViewModel<UserViewModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userExists = await _unitOfWork.User.UserExistsAsync(request.Email);

                if (userExists)
                {
                    return ResultViewModel<UserViewModel>.Error($"User with email {request.Email} already exists.");
                }

                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = Role.User,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddHours(-3)
                };

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.User.AddUserAsync(user);
                await _unitOfWork.CompleteAsync();

                return ResultViewModel<UserViewModel>.Success(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return ResultViewModel<UserViewModel>.Error(ex.Message);
            }
        }
    }
}
