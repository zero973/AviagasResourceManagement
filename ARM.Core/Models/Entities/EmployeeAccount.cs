using ARM.Core.Enums;

namespace ARM.Core.Models.Entities;

/// <summary>
/// Объединённая сущность <see cref="Employee"/> и <see cref="AppUser"/>.
/// </summary>
public class EmployeeAccount : BaseActualEntity
{
    
    /// <summary>
    /// Связанный сотрудник
    /// </summary>
    public Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Хэш пароля
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Роль в системе
    /// </summary>
    public UsersRoles Role { get; set; }
    
    /// <summary>
    /// ФИО
    /// </summary>
    public string FIO { get; set; }

    /// <summary>
    /// День рождения
    /// </summary>
    public DateTime Birthday { get; set; }
 
    /// <summary>
    /// Номер паспорта
    /// </summary>
    public string Passport { get; set; }
    
    /// <summary>
    /// Url картинки сотрудника
    /// </summary>
    public string? PhotoUrl { get; set; }
    
    /// <summary>
    /// Зарплата сотрудника в час
    /// </summary>
    public decimal SalaryForOneHour { get; set; }

    public EmployeeAccount()
    {
        
    }
    
    public EmployeeAccount(Guid employeeId, string login, UsersRoles role, string fio, 
        DateTime birthday, string passport, decimal salaryForOneHour, string? photoUrl)
    {
        EmployeeId = employeeId;
        Login = login;
        Role = role;
        FIO = fio;
        Birthday = birthday;
        Passport = passport;
        SalaryForOneHour = salaryForOneHour;
        PhotoUrl = photoUrl;
    }
}