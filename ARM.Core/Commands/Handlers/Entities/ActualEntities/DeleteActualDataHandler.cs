using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.ActualEntities;

public class DeleteActualDataHandler<T> : IRequestHandler<DeleteActualDataRequest<T>, Result<T>>
    where T : class, IActualEntity
{
    
    protected readonly IDbActualEntitiesRepository<T> _repository;
    protected readonly IUserIdentityProvider _authService;

    public DeleteActualDataHandler(IDbActualEntitiesRepository<T> repository, IUserIdentityProvider authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public virtual async Task<Result<T>> Handle(DeleteActualDataRequest<T> request, CancellationToken cancellationToken)
    {
        var result = await _repository.Remove(request.Id, _authService.GetCurrentUserIdentity().UserId);
        return new Result<T>(result.IsSuccess, null, result.Message);
    }

}