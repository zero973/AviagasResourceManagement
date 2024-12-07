using ARM.Core.Commands.Handlers.Entities.ActualEntities;
using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Helpers;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using FluentValidation;

namespace ARM.Core.Commands.Handlers.Entities.Concrete.Employee;

public class AddEmployeeHandler : AddActualDataHandler<EmployeeAccount>
{
    
    public AddEmployeeHandler(IDbEntitiesRepository<EmployeeAccount> repository, IValidator<EmployeeAccount> validator, 
            IUserIdentityProvider identityProvider) 
        : base(repository, validator, identityProvider)
    {
        
    }

    public override Task<Result<EmployeeAccount>> Handle(AddActualDataRequest<EmployeeAccount> request, 
        CancellationToken cancellationToken)
    {
        request.Entity.PasswordHash = Encryptor.EncryptString(request.Entity.Password);
        return base.Handle(request, cancellationToken);
    }
    
}