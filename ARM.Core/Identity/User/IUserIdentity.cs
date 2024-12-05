namespace ARM.Core.Identity.User;

/// <summary>
/// 
/// </summary>
public interface IUserIdentity
{
    
    /// <summary>
    /// 
    /// </summary>
    public string? AuthenticationType { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsAuthenticated { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; }
    
    /// <summary>
    /// 
    /// </summary>
    public Guid UserId { get; }
    
}