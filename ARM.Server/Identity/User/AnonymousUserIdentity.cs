using ARM.Core.Identity.User;

namespace ARM.WebApi.Identity.User;

/// <summary>
/// Identity that is returned if user is not authenticated
/// </summary>
public sealed class AnonymousUserIdentity : IUserIdentity
{
    public static readonly AnonymousUserIdentity Identity = new();
    
    public string AuthenticationType => "Unauthenticated";
    public bool IsAuthenticated => false;
    public string Name => "Anonymous";
    public Guid UserId => Guid.Empty;

    private AnonymousUserIdentity()
    {
        
    }
    
}