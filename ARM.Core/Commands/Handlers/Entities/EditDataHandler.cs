using ARM.Core.Commands.Requests.Entities;
using ARM.Core.Helpers;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Repositories;
using FluentResults;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Entities;

public class EditDataHandler<T> : IRequestHandler<EditDataRequest<T>, Result<T>>
    where T : class, IEntity
{

    protected readonly IDbEntitiesRepository<T> _repository;
    protected readonly IValidator<T> _validator;

    public EditDataHandler(IDbEntitiesRepository<T> repository, IValidator<T> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public virtual async Task<Result<T>> Handle(EditDataRequest<T> request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request.Entity, _validator);
        if (!validationResult.IsSuccess)
            return validationResult;

        return await _repository.Update(request.Entity);
    }

}