
using FluentValidation;
using revaly.auth.Application.Commands.UserCommand.UpdateUserCommand;

namespace revaly.auth.Application.Validators.UserValidators
{
    public class UpdateValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
