using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Helpers;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities;

public class AddDataHandler<T> : IRequestHandler<AddDataRequest<T>, Result<T>>
    where T : class, IEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;
    protected readonly IValidator<T> _validator;

    public AddDataHandler(IDbEntitiesRepository<T> repository, IValidator<T> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public virtual async Task<Result<T>> Handle(AddDataRequest<T> request, CancellationToken cancellationToken)
    {
        request.Entity.Id = Guid.NewGuid();
        
        var validationResult = await CommandHandlersHelper.Validate(request.Entity, _validator);
        if (!validationResult.IsSuccess)
            return validationResult;

        return await _repository.Add(request.Entity);
    }

}