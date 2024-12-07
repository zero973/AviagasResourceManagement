using ARM.Core.Models.Entities;
using ARM.Core.Validators.Helpers;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class EmployeeAccountValidator : AbstractValidator<EmployeeAccount>
{
    public EmployeeAccountValidator()
    {
        RuleFor(x => x.FIO).SetValidator(new FIOValidator());
        RuleFor(x => x.Passport).SetValidator(new PassportValidator());

        RuleFor(x => x.Birthday)
            .LessThan(DateTime.Now)
            .WithMessage("День рождения не может быть больше текущей даты");
        
        RuleFor(x => x.SalaryForOneHour)
            .GreaterThan(0)
            .WithMessage("Зарплата сотрудника должна быть больше 0");

        RuleFor(x => x.Login.Length)
            .GreaterThan(1)
            .WithMessage("Логин должен быть больше 1 символа");

        RuleFor(x => x.Password.Length)
            .GreaterThan(1)
            .WithMessage("Пароль должен быть больше 1 символа");
    }
}