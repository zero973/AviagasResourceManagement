namespace ARM.DAL.Models.Security;

public class RefreshToken
{
    
    public int Id { get; set; }

    /// <summary>
    /// Server-generated token Id used on client
    /// </summary>
    public string Token { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Access token Id that is related to this refresh token
    /// </summary>
    public string JwtId { get; set; } = null!;
    
    public bool IsUsed { get; set; }
    
    public bool IsRevoked { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public DateTime ExpiryDate { get; set; }
    
    public Guid UserId { get; set; }
    
    public string DeviceId { get; set; } = null!;
    
}