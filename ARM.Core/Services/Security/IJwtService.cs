using ARM.Core.Models.Entities;
using ARM.Core.Models.Security;

namespace ARM.Core.Services.Security;

/// <summary>
/// Интерфейс сервиса генерации JWT-токенов
/// </summary>
public interface IJwtService
{
    
    /// <summary>
    /// Сгенерировать пару токенов <see cref="TokensPair"/> для пользователя <paramref name="user"/>
    /// </summary>
    Task<TokensPair> GenerateTokenForUser(EmployeeAccount user, string deviceId);
    
}