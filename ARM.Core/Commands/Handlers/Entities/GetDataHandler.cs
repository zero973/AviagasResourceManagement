using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities;

public class GetDataHandler<T> : IRequestHandler<GetDataRequest<T>, Result<T>>
    where T : class, IEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;

    public GetDataHandler(IDbEntitiesRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<Result<T>> Handle(GetDataRequest<T> request, CancellationToken cancellationToken)
    {
        return await _repository.Get(request.Id);
    }

}