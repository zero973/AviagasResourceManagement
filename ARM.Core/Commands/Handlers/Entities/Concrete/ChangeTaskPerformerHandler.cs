using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Helpers;
using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.Concrete;

/// <summary>
/// Хэндлер для смены текущего исполнителя задачи
/// </summary>
public class ChangeTaskPerformerHandler : IRequestHandler<ChangeTaskPerformer, Result<SystemTask>>
{
    
    private readonly IDbEntitiesRepository<SystemTask> _repository;
    private readonly IValidator<ChangeTaskPerformer> _validator;

    public ChangeTaskPerformerHandler(IDbEntitiesRepository<SystemTask> repository, 
        IValidator<ChangeTaskPerformer> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<SystemTask>> Handle(ChangeTaskPerformer request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request, _validator);
        if (!validationResult.IsSuccess)
            return new Result<SystemTask>(false, null, validationResult.Message);

        var task = (await _repository.Get(request.TaskId)).Data;
        task.CurrentPerformerId = request.EmployeeId;
        
        return await _repository.Update(task);
    }
    
}