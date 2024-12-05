using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.ActualEntities;

public class GetActualAllDataHandler<T> : IRequestHandler<GetActualAllDataRequest<T>, Result<List<T>>>
    where T : class, IActualEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;

    public GetActualAllDataHandler(IDbEntitiesRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<Result<List<T>>> Handle(GetActualAllDataRequest<T> request, CancellationToken cancellationToken)
    {
        return await _repository.GetAll(request.BaseParams);
    }

}