using ARM.Core.Models.Entities;
using ARM.Core.Validators.Helpers;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.FirstName).SetValidator(new FIOValidator());
        RuleFor(x => x.LastName).SetValidator(new FIOValidator());
        RuleFor(x => x.Patronymic).SetValidator(new FIOValidator());

        RuleFor(x => x.Birthday)
            .LessThan(DateTime.Now)
            .WithMessage("День рождения не может быть больше текущей даты");

        RuleFor(x => x.Passport)
            .Length(10)
            .WithMessage("Паспортные данные должны иметь длину 10 символов - 4 числа и ещё 6 чисел, без пробелов и других символов");

        RuleFor(x => x.Passport)
            .Must((filed, passport) => passport.All(char.IsDigit))
            .WithMessage("Паспортные данные содержат некорректные символы");
        
        RuleFor(x => x.Passport)
            .Must(BeAValidPassport)
            .WithMessage("Паспортные данные содержат некорректны(й/е) символ(-ы)");
    }

    private bool BeAValidPassport(string passport)
    {
        var allowedSymbols = "1234567890 ";
        foreach (var c in passport)
            if (!allowedSymbols.Contains(c))
                return false;
        return true;
    }
    
}