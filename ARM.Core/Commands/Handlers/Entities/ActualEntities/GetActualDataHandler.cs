using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.ActualEntities;

public class GetActualDataHandler<T> : IRequestHandler<GetActualDataRequest<T>, Result<T>>
    where T : class, IActualEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;

    public GetActualDataHandler(IDbEntitiesRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<Result<T>> Handle(GetActualDataRequest<T> request, CancellationToken cancellationToken)
    {
        return await _repository.Get(request.Id);
    }

}