using Microsoft.IdentityModel.Tokens;

namespace ARM.Core.Settings;

/// <summary>
/// Настройки JWT
/// </summary>
public sealed class JwtSettings
{
    public required string SigningKey { get; init; } = null!;

    /// <summary>
    /// Set if <see cref="SecurityConfiguration.UseJwtEncryption"/> is set to true
    /// </summary>
    public string? EncryptionKey { get; init; }

    public string? Audience { get; init; }
    public string? Issuer { get; init; }
    public required TimeSpan AccessTokenLifetime { get; init; }
    public required int RefreshTokenMonthLifetime { get; init; }

    /// <summary>
    /// Token validation parameters
    /// </summary>
    public required TokenValidationParameters TokenValidationParameters { get; init; }
}