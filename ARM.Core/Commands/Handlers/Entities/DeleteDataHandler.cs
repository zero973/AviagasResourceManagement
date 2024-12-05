using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities;

public class DeleteDataHandler<T> : IRequestHandler<DeleteDataRequest<T>, Result<T>>
    where T : class, IEntity
{
    
    protected readonly IDbEntitiesRepository<T> _repository;

    public DeleteDataHandler(IDbEntitiesRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<Result<T>> Handle(DeleteDataRequest<T> request, CancellationToken cancellationToken)
    {
        return await _repository.Remove(request.Entity);
    }

}