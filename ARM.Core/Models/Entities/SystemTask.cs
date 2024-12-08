using ARM.Core.Enums;

namespace ARM.Core.Models.Entities;

/// <summary>
/// Задача на сбор шкафа
/// </summary>
public class SystemTask : BaseActualEntity
{
    
    /// <summary>
    /// Название задачи
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Собираемый шкаф
    /// </summary>
    public required Guid CabinetId { get; set; }
    
    /// <summary>
    /// Текущий статус задачи
    /// </summary>
    public TaskStatuses Status { get; set; }
    
    /// <summary>
    /// Текущий исполнитель задачи - Employee.Id
    /// </summary>
    public Guid? CurrentPerformerId { get; set; }

    /// <summary>
    /// Все сотрудники, которые принима(ют\ли) участие в задаче
    /// </summary>
    public required List<EmployeeAccount> Employees { get; set; }
    
    /// <summary>
    /// Крайний срок 
    /// </summary>
    public DateTime Deadline { get; set; }
    
    /// <summary>
    /// Дата закрытия задачи (когда она была выполнена)
    /// </summary>
    public DateTime? FinishDate { get; set; }
    
    /// <summary>
    /// Предполагаемые часы работы над задачей
    /// </summary>
    public int EstimatedWorkHours { get; set; }
    
    /// <summary>
    /// Отработанные часы работы над задачей
    /// </summary>
    public int RealWorkedHours { get; set; }
    
    /// <summary>
    /// Комментарии пользователей в задаче
    /// </summary>
    public List<Comment> Comments { get; set; } = new();
    
    /// <summary>
    /// Детали, которые нужны для выполнения задачи.
    /// Key - деталь шкафа, ключ - количество.
    /// </summary>
    public List<CabinetPartCounts> CabinetParts { get; set; } = new();
    
}