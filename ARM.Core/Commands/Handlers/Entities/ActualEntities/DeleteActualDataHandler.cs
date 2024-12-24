using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Repositories;
using FluentResults;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.ActualEntities;

public class DeleteActualDataHandler<T> : IRequestHandler<DeleteActualDataRequest<T>, Result>
    where T : class, IActualEntity
{
    
    protected readonly IDbActualEntitiesRepository<T> _repository;
    protected readonly IUserIdentityProvider _authService;

    public DeleteActualDataHandler(IDbActualEntitiesRepository<T> repository, IUserIdentityProvider authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public virtual async Task<Result> Handle(DeleteActualDataRequest<T> request, CancellationToken cancellationToken)
    {
        return await _repository.Remove(request.Id, _authService.GetCurrentUserIdentity().UserId);
    }

}