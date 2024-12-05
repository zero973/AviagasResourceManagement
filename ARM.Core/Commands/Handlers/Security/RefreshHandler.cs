using ARM.Core.Commands.Requests.Security;
using ARM.Core.Helpers;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using ARM.Core.Services.Security;
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
        if (!validationResult.IsSuccess)
            return new Result<TokensPair>(validationResult.Message);
        
        return await _authorizationService.Refresh(request.Pair, request.DeviceId);
    }
    
}