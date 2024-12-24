using ARM.Core.Commands.Requests.Security;
using ARM.Core.Helpers;
using ARM.Core.Models.Security;
using ARM.Core.Services.Security;
using FluentResults;
using FluentValidation;
using MediatR;

namespace ARM.Core.Commands.Handlers.Security;

public class RefreshHandler : IRequestHandler<RefreshRequest, Result<TokensPair>>
{

    private readonly IAuthorizationService _authorizationService;
    private readonly IValidator<TokensPair> _validator;

    public RefreshHandler(IAuthorizationService authorizationService, IValidator<TokensPair> validator)
    {
        _authorizationService = authorizationService;
        _validator = validator;
    }

    public async Task<Result<TokensPair>> Handle(RefreshRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await CommandHandlersHelper.Validate(request.Pair, _validator);
        if (validationResult.IsFailed)
            return Result.Fail<TokensPair>(validationResult.Errors);
        
        return await _authorizationService.Refresh(request.Pair, request.DeviceId);
    }
    
}