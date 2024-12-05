using ARM.Core.Models.Entities;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class AppUserValidator : AbstractValidator<AppUser>
{
    public AppUserValidator()
    {
        RuleFor(x => x.Login).MinimumLength(4)
            .WithMessage("Логин должен быть больше 4 символов");
        
        RuleFor(x => x.PasswordHash).MinimumLength(1)
            .WithMessage("Пароль должен быть больше 1 символа");
    }
}