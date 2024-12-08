using System.ComponentModel.DataAnnotations.Schema;
using ARM.DAL.Constants;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Отработанное время сотрудника
/// </summary>
[Table(DbConstants.WorkedTimeTableName, Schema = DbConstants.DataSchema)]
public class WorkedTime : BaseActualEntity
{
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Employee Employee { get; set; }
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Задача, в которй был оставлен коментарий
    /// </summary>
    public SystemTask LinkedTask { get; set; }
    
    /// <summary>
    /// Задача, в которй был оставлен коментарий
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// День, за который пользователь списывает время
    /// </summary>
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Кол-во отработанных часов
    /// </summary>
    public int Hours { get; set; }
    
    /// <summary>
    /// Переработка
    /// </summary>
    public bool IsOverwork { get; set; }

    public WorkedTime()
    {
        
    }

    public WorkedTime(Guid employeeId, Guid taskId, DateTime date, int hours, bool isOverwork)
    {
        EmployeeId = employeeId;
        TaskId = taskId;
        Date = date;
        Hours = hours;
        IsOverwork = isOverwork;
    }
    
}