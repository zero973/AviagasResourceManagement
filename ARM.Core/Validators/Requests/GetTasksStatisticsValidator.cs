using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Extensions;
using FluentValidation;

namespace ARM.Core.Validators.Requests;

public class GetTasksStatisticsValidator : AbstractValidator<GetTasksStatistics>
{
    public GetTasksStatisticsValidator()
    {
        RuleFor(x => x.Month)
            .GreaterThan(DateTime.Now.WithLastDayMonth())
            .WithMessage("Месяц не должен превышать текущую дату");
    }
}