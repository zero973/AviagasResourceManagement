namespace ARM.Core.Models.Security;

/// <summary>
/// Пара JWT токенов - AccessToken и RefreshToken.
/// </summary>
public class TokensPair
{

    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }

    public TokensPair()
    {
        
    }
    
    public TokensPair(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    
}