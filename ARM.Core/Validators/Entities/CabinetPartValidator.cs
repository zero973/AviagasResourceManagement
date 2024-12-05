using ARM.Core.Models.Entities;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class CabinetPartValidator : AbstractValidator<CabinetPart>
{
    public CabinetPartValidator()
    {
        RuleFor(x => x.Name).MinimumLength(1)
            .WithMessage("Название должно быть больше 1 символа");
    }
}