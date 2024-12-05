using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ARM.Core.Settings;
using Microsoft.IdentityModel.Tokens;

namespace ARM.Core.Helpers;

public static class JwtUtils
{
    
    /// <summary>
    /// Get information about user who was given the access token
    /// </summary>
    public static ClaimsPrincipal? GetPrincipalFromToken(string token,
        TokenValidationParameters tokenValidationParameters)
    {
        var updatedTokenValidationParameters = tokenValidationParameters.Clone();
        updatedTokenValidationParameters.ValidateLifetime = false;
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, updatedTokenValidationParameters, out var validatedToken);
            return !IsJwtHasValidSecurityAlgorithm(validatedToken) ? null : principal;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsJwtHasValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken token &&
               (token.Header.Enc is not null
                   ? token.Header.Alg.Equals(EncryptionConstants.ValidEncryptionAlgorithm,
                       StringComparison.InvariantCultureIgnoreCase)
                   : token.Header.Alg.Equals(EncryptionConstants.ValidAlgorithm, StringComparison.InvariantCulture)
               );
    }
    
}