using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Models.Entities;
using ARM.Core.Repositories;
using FluentValidation;

namespace ARM.Core.Validators.Requests;

public class ChangeTaskPerformerValidator : AbstractValidator<ChangeTaskPerformer>
{
    public ChangeTaskPerformerValidator(IDbActualEntitiesRepository<SystemTask> taskRepository, 
        IDbActualEntitiesRepository<EmployeeAccount> employeeRepository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x)
            .MustAsync(async (field, token) =>
            {
                var result = await taskRepository.Get(field.TaskId);
                return result.IsSuccess;
            })
            .WithMessage("Не существует задачи с таким Id. Проверьте правильность идентификатора задачи.");
        
        RuleFor(x => x)
            .MustAsync(async (field, token) =>
            {
                var result = await employeeRepository.Get(field.EmployeeId);
                return result.IsSuccess;
            })
            .WithMessage("Не существует сотрудника с таким Id. Проверьте правильность идентификатора сотрудника.");
    }
}