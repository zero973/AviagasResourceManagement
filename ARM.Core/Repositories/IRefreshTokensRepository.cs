using ARM.Core.Models.Security;

namespace ARM.Core.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с RefreshTokens
/// </summary>
public interface IRefreshTokensRepository
{

    /// <summary>
    /// Получить токен
    /// </summary>
    Task<RefreshToken> GetToken(string token);
    
    /// <summary>
    /// Создать токен
    /// </summary>
    Task CreateToken(RefreshToken token);
    
    /// <summary>
    /// Использовать токен
    /// </summary>
    Task UseToken(string token);
    
    /// <summary>
    /// Отозвать токен
    /// </summary>
    Task RevokeTokenIfExists(Guid userId, string deviceId);
    
}