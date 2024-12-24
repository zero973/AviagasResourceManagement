using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using FluentResults;

namespace ARM.Core.Services.Security;

/// <summary>
/// Сервис 
/// </summary>
public interface IAuthorizationService
{

    /// <summary>
    /// Войти по логину и паролю
    /// </summary>
    Task<Result<TokensPair>> LogIn(LogInCredentials credentials, string deviceId);
    
    /// <summary>
    /// Зарегистрироваться
    /// </summary>
    Task<Result<TokensPair>> SignUp(SignUpCredentials credentials, string deviceId);

    /// <summary>
    /// Обновить Access токен
    /// </summary>
    Task<Result<TokensPair>> Refresh(TokensPair pair, string deviceId);

}