using ARM.Core.Models.Entities;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class WorkedTimeValidator : AbstractValidator<WorkedTime>
{
    public WorkedTimeValidator()
    {
        RuleFor(x => x.Hours).GreaterThan(0)
            .WithMessage("Кол-во отработанных часов должно быть больше 0");
    }
}