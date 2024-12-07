using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Extensions;
using ARM.Core.Models.Statistics;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Task;

public class GetTasksStatisticsHandler : IRequestHandler<GetTasksStatistics, Result<List<TasksStatistics>>>
{

    private readonly IStatisticsRepository _repository;

    public GetTasksStatisticsHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<List<TasksStatistics>>> Handle(GetTasksStatistics request, CancellationToken cancellationToken)
    {
        if (request.Month > DateTime.Now.WithLastDayMonth())
        {
            return new Result<List<TasksStatistics>>("Месяц не должен превышать текущую дату");
        }
        return await _repository.GetTasksStatisticsForMonth(request.Month);
    }
    
}