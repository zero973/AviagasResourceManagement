using ARM.Core.Identity.User;

namespace ARM.Core.Identity.Providers;

/// <summary>
/// 
/// </summary>
public interface IUserIdentityProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IUserIdentity GetCurrentUserIdentity();
}