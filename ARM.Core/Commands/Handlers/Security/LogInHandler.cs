using ARM.Core.Commands.Requests.Security;
using ARM.Core.Helpers;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using ARM.Core.Services.Security;
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
        if (!validationResult.IsSuccess)
            return new Result<TokensPair>(validationResult.Message);
        
        return await _authorizationService.LogIn(request.Credentials, request.DeviceId);
    }
    
}