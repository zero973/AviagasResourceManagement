using ARM.Core.Models.Entities;
using FluentValidation;

namespace ARM.Core.Validators.Entities;

public class CabinetPartCountsValidator : AbstractValidator<CabinetPartCounts>
{
    public CabinetPartCountsValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Количество должно быть больше нуля");
    }
}