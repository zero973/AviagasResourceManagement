using ARM.Core.Models.Security;
using FluentValidation;

namespace ARM.Core.Validators.Security;

public class LogInCredentialsValidator : AbstractValidator<LogInCredentials>
{
    public LogInCredentialsValidator()
    {
        RuleFor(x => x.Login).MinimumLength(4)
            .WithMessage("Логин должен быть больше 4 символов");
        
        RuleFor(x => x.Password).MinimumLength(4)
            .WithMessage("Пароль должен быть больше 4 символов");
    }
}