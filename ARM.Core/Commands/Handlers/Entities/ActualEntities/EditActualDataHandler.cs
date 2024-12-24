using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Helpers;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Repositories;
using FluentResults;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.ActualEntities;

public class EditActualDataHandler<T> : IRequestHandler<EditActualDataRequest<T>, Result<T>>
    where T : class, IActualEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;
    protected readonly IValidator<T> _validator;
    protected readonly IUserIdentityProvider _authService;

    public EditActualDataHandler(IDbEntitiesRepository<T> repository, IValidator<T> validator, 
        IUserIdentityProvider authService)
    {
        _repository = repository;
        _validator = validator;
        _authService = authService;
    }

    public virtual async Task<Result<T>> Handle(EditActualDataRequest<T> request, CancellationToken cancellationToken)
    {
        request.Entity.UpdateDate = DateTime.Now;
        request.Entity.UpdatedUserId = _authService.GetCurrentUserIdentity().UserId;
        
        var validationResult = await CommandHandlersHelper.Validate(request.Entity, _validator);
        if (!validationResult.IsSuccess)
            return validationResult;

        return await _repository.Update(request.Entity);
    }

}