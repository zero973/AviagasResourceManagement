﻿using ARM.Core.Models.Entities;
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
        RuleFor(x => x.Passport).SetValidator(new PassportValidator());

        RuleFor(x => x.Birthday)
            .LessThan(DateTime.Now)
            .WithMessage("День рождения не может быть больше текущей даты");
        
        RuleFor(x => x.SalaryForOneHour)
            .GreaterThan(0)
            .WithMessage("Зарплата сотрудника должна быть больше 0");
    }
    
}