using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ARM.Core.Models.Entities;
using ARM.Core.Models.Security;
using ARM.Core.Repositories;
using ARM.Core.Settings;
using Microsoft.IdentityModel.Tokens;

namespace ARM.Core.Services.Security.Impl;

/// <summary>
/// Сервис генерации JWT-токенов
/// </summary>
public class JwtService : IJwtService
{
    
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokensRepository _refreshTokensRepository;

    public JwtService(JwtSettings jwtSettings, IRefreshTokensRepository refreshTokensRepository)
    {
        _jwtSettings = jwtSettings;
        _refreshTokensRepository = refreshTokensRepository;
    }

    public async Task<TokensPair> GenerateTokenForUser(EmployeeAccount user, string deviceId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Login),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Name, user.Login),
            new(JwtRegisteredClaimNames.Email, user.Login),
            new("id", user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience ?? default,
            Issuer = _jwtSettings.Issuer ?? default,
            IssuedAt = DateTime.UtcNow,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtSettings.AccessTokenLifetime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var encryptingKey = Encoding.ASCII.GetBytes(_jwtSettings.EncryptionKey!);
        tokenDescriptor.EncryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptingKey),
            JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = new RefreshToken
        {
            JwtId = accessToken.Id,
            UserId = user.Id,
            DeviceId = deviceId,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenMonthLifetime)
        };

        await _refreshTokensRepository.CreateToken(refreshToken);

        return new TokensPair(tokenHandler.WriteToken(accessToken), refreshToken.Token);
    }
}