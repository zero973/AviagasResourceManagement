namespace ARM.Core.Models.Entities;

/// <summary>
/// Сотрудник
/// </summary>
public class Employee : BaseActualEntity
{

    /// <summary>
    /// Имя
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public required string Patronymic { get; set; }

    /// <summary>
    /// День рождения
    /// </summary>
    public DateTime Birthday { get; set; }
 
    /// <summary>
    /// Номер паспорта
    /// </summary>
    public required string Passport { get; set; }
    
    /// <summary>
    /// Url картинки сотрудника
    /// </summary>
    public string? PhotoUrl { get; set; }
    
    /// <summary>
    /// Зарплата сотрудника в час
    /// </summary>
    public decimal SalaryForOneHour { get; set; }
    
}