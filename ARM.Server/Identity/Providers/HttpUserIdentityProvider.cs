using ARM.Core.Identity.Providers;
using ARM.Core.Identity.User;
using ARM.WebApi.Identity.User;

namespace ARM.WebApi.Identity.Providers;

/// <summary>
/// 
/// </summary>
public class HttpUserIdentityProvider : IUserIdentityProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpUserIdentityProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public IUserIdentity GetCurrentUserIdentity()
    {
        var contextIdentity = _contextAccessor.HttpContext?.User;

        if (contextIdentity?.Identity is null || !contextIdentity.Identity.IsAuthenticated)
            return AnonymousUserIdentity.Identity;

        var userIdParseResult = Guid.TryParse(contextIdentity.Claims.First(x => x.Type == "id").Value, out var userId);
        if (!userIdParseResult)
            return AnonymousUserIdentity.Identity;

        return new UserIdentity(contextIdentity.Identity, userId);
    }
    
}