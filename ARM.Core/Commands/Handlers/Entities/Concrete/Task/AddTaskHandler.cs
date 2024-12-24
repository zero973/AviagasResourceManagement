using ARM.Core.Commands.Handlers.Entities.ActualEntities;
using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Enums;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities;
using ARM.Core.Repositories;
using FluentResults;
using FluentValidation;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Task;

public class AddTaskHandler : AddActualDataHandler<SystemTask>
{
    
    public AddTaskHandler(IDbActualEntitiesRepository<SystemTask> repository, IValidator<SystemTask> validator, 
            IUserIdentityProvider identityProvider) 
        : base(repository, validator, identityProvider)
    {
        
    }

    public override Task<Result<SystemTask>> Handle(AddActualDataRequest<SystemTask> request, CancellationToken cancellationToken)
    {
        // при создании задачи автоматически назначаем её на чертёжника
        request.Entity.CurrentPerformerId = request.Entity.Employees.Single(x => x.Role == UsersRoles.Draftsman).Id;
        
        return base.Handle(request, cancellationToken);
    }
    
}