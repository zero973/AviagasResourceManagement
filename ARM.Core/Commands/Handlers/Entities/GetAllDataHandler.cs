using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities;

public class GetAllDataHandler<T> : IRequestHandler<GetAllDataRequest<T>, Result<List<T>>>
    where T : class, IEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;

    public GetAllDataHandler(IDbEntitiesRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<Result<List<T>>> Handle(GetAllDataRequest<T> request, CancellationToken cancellationToken)
    {
        return await _repository.GetAll(request.BaseParams);
    }

}