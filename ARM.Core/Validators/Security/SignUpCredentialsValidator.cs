using ARM.Core.Models.UI;
using FluentValidation;

namespace ARM.Core.Validators.Security;

public class SignUpCredentialsValidator : AbstractValidator<SignUpCredentials>
{
    public SignUpCredentialsValidator()
    {
        RuleFor(x => x.Login).MinimumLength(4)
            .WithMessage("Логин должен быть больше 4 символов");
        
        RuleFor(x => x.Password).MinimumLength(4)
            .WithMessage("Пароль должен быть больше 4 символов");
    }
}