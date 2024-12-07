using FluentValidation;

namespace ARM.Core.Validators.Helpers;

public class PassportValidator: AbstractValidator<string>
{
    public PassportValidator()
    {
        RuleFor(x => x)
            .Length(10)
            .WithMessage("Паспортные данные должны иметь длину 10 символов - 4 числа и ещё 6 чисел, без пробелов и других символов");

        RuleFor(x => x)
            .Must((filed, passport) => passport.All(char.IsDigit))
            .WithMessage("Паспортные данные содержат некорректные символы");
    }
}