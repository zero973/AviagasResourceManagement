using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Helpers;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities.ActualEntities;

public class AddActualDataHandler<T> : IRequestHandler<AddActualDataRequest<T>, Result<T>>
    where T : class, IActualEntity
{

    protected readonly IDbActualEntitiesRepository<T> _repository;
    protected readonly IValidator<T> _validator;
    protected readonly IUserIdentityProvider _identityProvider;

    public AddActualDataHandler(IDbActualEntitiesRepository<T> repository, IValidator<T> validator, 
        IUserIdentityProvider identityProvider)
    {
        _repository = repository;
        _validator = validator;
        _identityProvider = identityProvider;
    }

    public virtual async Task<Result<T>> Handle(AddActualDataRequest<T> request, CancellationToken cancellationToken)
    {
        request.Entity.Id = Guid.NewGuid();
        request.Entity.CreateDate = DateTime.Now;
        request.Entity.CreatedUserId = _identityProvider.GetCurrentUserIdentity().UserId;
        
        var validationResult = await CommandHandlersHelper.Validate(request.Entity, _validator);
        if (!validationResult.IsSuccess)
            return validationResult;

        return await _repository.Add(request.Entity);
    }

}