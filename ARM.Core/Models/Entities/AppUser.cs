using ARM.Core.Enums;

namespace ARM.Core.Models.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public class AppUser : BaseActualEntity
{

    /// <summary>
    /// Логин
    /// </summary>
    public required string Login { get; set; }
    
    /// <summary>
    /// Хэш пароля
    /// </summary>
    public required string PasswordHash { get; set; }
    
    /// <summary>
    /// Роль в системе
    /// </summary>
    public UsersRoles Role { get; set; }
    
    /// <summary>
    /// Связанный сотрудник
    /// </summary>
    public required Guid EmployeeId { get; set; }
    
}