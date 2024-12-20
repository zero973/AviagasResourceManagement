using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Models.Entities;
using ARM.Core.Repositories;
using FluentValidation;

namespace ARM.Core.Validators.Requests;

public class ChangeTaskStatusValidator : AbstractValidator<ChangeTaskStatus>
{
    public ChangeTaskStatusValidator(IDbActualEntitiesRepository<SystemTask> repository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x)
            .MustAsync(async (field, token) =>
            {
                var result = await repository.Get(field.TaskId);
                return result.IsSuccess;
            })
            .WithMessage("Не существует задачи с таким Id. Проверьте правильность идентификатора задачи.");
        
        RuleFor(x => x)
            .MustAsync(async (field, token) =>
            {
                var task = (await repository.Get(field.TaskId)).Data;
                return (int)task.Status > (int)field.NewStatus;
            })
            .WithMessage("Нельзя понизить статус задачи. Проверьте правильность устанавливаемого статуса.");
    }
}