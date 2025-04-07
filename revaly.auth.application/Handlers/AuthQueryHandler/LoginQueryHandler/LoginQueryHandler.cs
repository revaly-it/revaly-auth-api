using MediatR;
using revaly.auth.Application.Models;
using revaly.auth.Application.Queries.LoginQuery;
using revaly.auth.Application.Services.Interface;
using revaly.auth.Domain.Interfaces.IUnitOfWork;

namespace revaly.auth.Application.Handlers.AuthQueryHandler.LoginQueryHandler
{
    public class LoginQueryHandler (
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<LoginQuery, ResultViewModel<string>>
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultViewModel<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.GetUserByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return ResultViewModel<string>.Error("Invalid email or password");
            }

            var token = _tokenService.GenerateToken(user);
            return ResultViewModel<string>.Success(token);
        }
    }
}
