using ARM.Core.Commands.Requests.Entities.Concrete;
using ARM.Core.Models.Statistics;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Statistics;

public class GetPlainExpensesHandler : IRequestHandler<GetPlainExpenses, Result<List<PlainExpensesStatistics>>>
{

    private readonly IStatisticsRepository _repository;

    public GetPlainExpensesHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<List<PlainExpensesStatistics>>> Handle(GetPlainExpenses request, CancellationToken cancellationToken)
    {
        return await _repository.GetPlainExpenses(request.Counts);
    }
    
}