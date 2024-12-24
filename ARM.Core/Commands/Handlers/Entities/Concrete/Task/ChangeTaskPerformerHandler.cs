using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Helpers;
using ARM.Core.Models.Entities;
using ARM.Core.Repositories;
using FluentResults;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Task;

/// <summary>
/// Хэндлер для смены текущего исполнителя задачи
/// </summary>
public class ChangeTaskPerformerHandler : IRequestHandler<ChangeTaskPerformer, Result<SystemTask>>
{
    
    private readonly IDbActualEntitiesRepository<SystemTask> _repository;
    private readonly IValidator<ChangeTaskPerformer> _validator;

    public ChangeTaskPerformerHandler(IDbActualEntitiesRepository<SystemTask> repository, 
        IValidator<ChangeTaskPerformer> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<SystemTask>> Handle(ChangeTaskPerformer request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request, _validator);
        if (validationResult.IsFailed)
            return Result.Fail<SystemTask>(validationResult.Errors);

        var task = (await _repository.Get(request.TaskId)).Value;
        task.CurrentPerformerId = request.EmployeeId;
        
        return await _repository.Update(task);
    }
    
}