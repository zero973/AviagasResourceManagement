using ARM.Core.Commands.Requests.Security;
using ARM.Core.Helpers;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using ARM.Core.Services.Security;
using FluentResults;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Security;

public class LogInHandler : IRequestHandler<LogInRequest, Result<TokensPair>>
{

    private readonly IAuthorizationService _authorizationService;
    private readonly IValidator<LogInCredentials> _validator;

    public LogInHandler(IAuthorizationService authorizationService, IValidator<LogInCredentials> validator)
    {
        _authorizationService = authorizationService;
        _validator = validator;
    }

    public async Task<Result<TokensPair>> Handle(LogInRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request.Credentials, _validator);
        if (validationResult.IsFailed)
            return Result.Fail<TokensPair>(validationResult.Errors);
        
        return await _authorizationService.LogIn(request.Credentials, request.DeviceId);
    }
    
}