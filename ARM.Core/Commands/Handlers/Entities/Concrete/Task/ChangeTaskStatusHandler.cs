using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Enums;
using ARM.Core.Helpers;
using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Task;

/// <summary>
/// Хэндлер для смены статуса задачи и автоматической смены нужного исполнителя в зависимости от новго статуса.
/// </summary>
public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatus, Result<SystemTask>>
{
    
    private readonly IDbEntitiesRepository<SystemTask> _repository;
    private readonly IValidator<ChangeTaskStatus> _validator;

    public ChangeTaskStatusHandler(IDbEntitiesRepository<SystemTask> repository, 
        IValidator<ChangeTaskStatus> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<SystemTask>> Handle(ChangeTaskStatus request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request, _validator);
        if (!validationResult.IsSuccess)
            return new Result<SystemTask>(false, null, validationResult.Message);

        var task = (await _repository.Get(request.TaskId)).Data;
        
        // проставляем новый статус
        task.Status = request.NewStatus;
        
        // меняем исполнителя в зависимости от нового статуса
        if (task.Status == TaskStatuses.AssemblingComponents)
            task.CurrentPerformerId = task.Employees.Single(x => x.Role == UsersRoles.Storage).Id;
        else if (task.Status == TaskStatuses.Installation)
            task.CurrentPerformerId = task.Employees.Single(x => x.Role == UsersRoles.Engineer).Id;
        
        return await _repository.Update(task);
    }
    
}