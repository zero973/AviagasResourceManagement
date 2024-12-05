using ARM.Core.Commands.Requests.Security;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARM.WebApi.Controllers.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    
    private readonly ISender _sender;
    private readonly IUserIdentityProvider _userIdentityProvider;

    public AuthorizationController(ISender sender, 
        IUserIdentityProvider userIdentityProvider)
    {
        _sender = sender;
        _userIdentityProvider = userIdentityProvider;
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> LogIn([FromBody] LogInCredentials credentials)
    {
        var result = await _sender.Send(new LogInRequest(credentials, Request.Headers.UserAgent!));
        return new JsonResult(result);
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> SignUp([FromBody] SignUpCredentials credentials)
    {
        var result = await _sender.Send(new SignUpRequest(credentials, Request.Headers.UserAgent!));
        return new JsonResult(result);
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> Refresh([FromBody] TokensPair pair)
    {
        var result = await _sender.Send(new RefreshRequest(pair, Request.Headers.UserAgent!));
        return new JsonResult(result);
    }
    
    [Authorize]
    [HttpPost]
    [Route("[action]")]
    public async Task<JsonResult> GetCurrentUserId([FromBody] TokensPair pair)
    {
        return new JsonResult(_userIdentityProvider.GetCurrentUserIdentity().UserId);
    }
    
}