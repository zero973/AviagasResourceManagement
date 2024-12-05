using ARM.Core.Commands.Requests.Security;
using ARM.Core.Helpers;
using ARM.Core.Models.Entities;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using ARM.Core.Services.Security;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Security;

public class SignUpHandler : IRequestHandler<SignUpRequest, Result<TokensPair>>
{

    private readonly IAuthorizationService _authorizationService;
    private readonly IValidator<SignUpCredentials> _validator;
    
    public SignUpHandler(IAuthorizationService authorizationService, IValidator<SignUpCredentials> validator)
    {
        _authorizationService = authorizationService;
        _validator = validator;
    }

    public async Task<Result<TokensPair>> Handle(SignUpRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request.Credentials, _validator);
        if (!validationResult.IsSuccess)
            return new Result<TokensPair>(validationResult.Message);
        
        return await _authorizationService.SignUp(request.Credentials, request.DeviceId);
    }
    
}