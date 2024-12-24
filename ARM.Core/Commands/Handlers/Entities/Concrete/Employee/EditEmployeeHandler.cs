using ARM.Core.Commands.Handlers.Entities.ActualEntities;
using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Helpers;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities;
using ARM.Core.Repositories;
using FluentResults;
using FluentValidation;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Employee;

public class EditEmployeeHandler : EditActualDataHandler<EmployeeAccount>
{
    
    public EditEmployeeHandler(IDbActualEntitiesRepository<EmployeeAccount> repository, IValidator<EmployeeAccount> validator, 
        IUserIdentityProvider authService) : base(repository, validator, authService)
    {
        
    }

    public override Task<Result<EmployeeAccount>> Handle(EditActualDataRequest<EmployeeAccount> request, 
        CancellationToken cancellationToken)
    {
        request.Entity.PasswordHash = Encryptor.EncryptString(request.Entity.Password);
        return base.Handle(request, cancellationToken);
    }
    
}