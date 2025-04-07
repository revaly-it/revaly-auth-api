using MediatR;
using revaly.auth.Application.Models;

namespace revaly.auth.Application.Queries.LoginQuery
{
    public class LoginQuery : IRequest<ResultViewModel<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
