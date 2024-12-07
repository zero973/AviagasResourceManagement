namespace ARM.Core.Models.Entities;

/// <summary>
/// Отработанное время сотрудника
/// </summary>
public class WorkedTime : BaseActualEntity
{

    /// <summary>
    /// Пользователь
    /// </summary>
    public required Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Задача, над которой работал сотрудник
    /// </summary>
    public required Guid TaskId { get; set; }

    /// <summary>
    /// День, за который пользователь списывает время
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Кол-во отработанных часов
    /// </summary>
    public int Hours { get; set; }
    
    /// <summary>
    /// Переработка
    /// </summary>
    public bool IsOverwork { get; set; }
    
}