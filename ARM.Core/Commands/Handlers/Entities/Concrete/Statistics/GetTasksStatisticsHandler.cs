using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Extensions;
using ARM.Core.Helpers;
using ARM.Core.Models.Statistics;
using ARM.Core.Repositories;
using FluentResults;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Statistics;

public class GetTasksStatisticsHandler : IRequestHandler<GetTasksStatistics, Result<List<TasksStatistics>>>
{

    private readonly IStatisticsRepository _repository;
    private readonly IValidator<GetTasksStatistics> _validator;

    public GetTasksStatisticsHandler(IStatisticsRepository repository, IValidator<GetTasksStatistics> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<Result<List<TasksStatistics>>> Handle(GetTasksStatistics request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request, _validator);
        if (validationResult.IsFailed)
            return Result.Fail<List<TasksStatistics>>(validationResult.Errors);
        
        return await _repository.GetTasksStatisticsForMonth(request.Month);
    }
    
}