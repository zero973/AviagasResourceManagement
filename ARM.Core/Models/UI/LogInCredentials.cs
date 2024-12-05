namespace ARM.Core.Models.Security;

/// <summary>
/// Даныые для входа
/// </summary>
public class LogInCredentials
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}