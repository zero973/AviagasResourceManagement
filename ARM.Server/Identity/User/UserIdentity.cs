using System.Security.Principal;
using ARM.Core.Identity.User;

namespace ARM.WebApi.Identity.User;

/// <summary>
/// Identity that is returned if user is authenticated
/// </summary>
public class UserIdentity : IUserIdentity
{
    
    public string? AuthenticationType { get; }
    
    public bool IsAuthenticated { get; }
    
    public string? Name { get; }
    
    public Guid UserId { get; }

    public UserIdentity(IIdentity identity, Guid userId)
    {
        AuthenticationType = identity.AuthenticationType;
        IsAuthenticated = identity.IsAuthenticated;
        Name = identity.Name;
        UserId = userId;
    }
    
}