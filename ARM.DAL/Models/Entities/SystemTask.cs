using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ARM.Core.Enums;
using ARM.DAL.Constants;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Задача на сбор шкафа
/// </summary>
[Table(DbConstants.TaskTableName, Schema = DbConstants.DataSchema)]
public class SystemTask : BaseActualEntity
{
    
    /// <summary>
    /// Название задачи
    /// </summary>
    [MaxLength(150)]
    public string Name { get; set; }

    /// <summary>
    /// Собираемый шкаф
    /// </summary>
    public Cabinet Cabinet { get; set; }

    /// <summary>
    /// Собираемый шкаф
    /// </summary>
    public Guid CabinetId { get; set; }
    
    /// <summary>
    /// Текущий статус задачи
    /// </summary>
    public TaskStatuses Status { get; set; }
    
    /// <summary>
    /// Текущий исполнитель задачи
    /// </summary>
    public Employee? CurrentPerformer { get; set; }

    /// <summary>
    /// Текущий исполнитель задачи
    /// </summary>
    public Guid? CurrentPerformerId { get; set; }
    
    /// <summary>
    /// Крайний срок
    /// </summary>
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Deadline { get; set; }
    
    /// <summary>
    /// Дата закрытия задачи
    /// </summary>
    [Column(TypeName = "timestamp without time zone")]
    public DateTime? FinishDate { get; set; }
    
    /// <summary>
    /// Предполагаемые часы работы над задачей
    /// </summary>
    public int EstimatedWorkHours { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    
    public ICollection<WorkedTime> WorkedTimes { get; set; }
    
    public ICollection<TaskEmployee> TaskEmployees { get; set; }
    
    public ICollection<CabinetPartCounts> CabinetPartCounts { get; set; }

    public SystemTask()
    {
        
    }

    public SystemTask(string name, Guid cabinetId, TaskStatuses status, Guid? currentPerformerId, DateTime deadline, int estimatedWorkHours)
    {
        Name = name;
        CabinetId = cabinetId;
        Status = status;
        CurrentPerformerId = currentPerformerId;
        Deadline = deadline;
        EstimatedWorkHours = estimatedWorkHours;
    }
}