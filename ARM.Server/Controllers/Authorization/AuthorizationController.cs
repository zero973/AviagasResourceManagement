using ARM.Core.Commands.Requests.Security;
using ARM.Core.Identity.Providers;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using FluentResults.Extensions.AspNetCore;
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
    
    [AllowAnonymous]
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<TokensPair>> LogIn([FromBody] LogInCredentials credentials)
    {
        return await _sender.Send(new LogInRequest(credentials, Request.Headers.UserAgent!)).ToActionResult();
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<TokensPair>> SignUp([FromBody] SignUpCredentials credentials)
    {
        return await _sender.Send(new SignUpRequest(credentials, Request.Headers.UserAgent!)).ToActionResult();
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<TokensPair>> Refresh([FromBody] TokensPair pair)
    {
        return await _sender.Send(new RefreshRequest(pair, Request.Headers.UserAgent!)).ToActionResult();
    }
    
    [Authorize]
    [HttpPost]
    [Route("[action]")]
    public ActionResult<Guid> GetCurrentUserId()
    {
        return _userIdentityProvider.GetCurrentUserIdentity().UserId;
    }
    
}