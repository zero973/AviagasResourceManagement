using ARM.Core.Models.Entities;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class SystemTaskValidator : AbstractValidator<SystemTask>
{
    public SystemTaskValidator()
    {
        RuleFor(x => x.Name).MinimumLength(1)
            .WithMessage("Название задачи должно быть больше 1 символа");
    }
}