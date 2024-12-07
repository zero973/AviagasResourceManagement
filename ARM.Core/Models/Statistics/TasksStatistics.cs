namespace ARM.Core.Models.Statistics;

/// <summary>
/// Статистика по затратам на одну задачу
/// </summary>
public class TasksStatistics
{
    
    /// <summary>
    /// Id задачи
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// Название задачи
    /// </summary>
    public string TaskName { get; set; }
    
    /// <summary>
    /// Текущий статус задачи
    /// </summary>
    public TaskStatus Status { get; set; }
    
    /// <summary>
    /// Оценка (предполагаемые часы работы над задачей)
    /// </summary>
    public int EstimatedWorkHours { get; set; }
    
    /// <summary>
    /// Кол-во затраченных часов
    /// </summary>
    public int RealWorkedHours { get; set; }
    
    /// <summary>
    /// Суммарные затраты на сотрудников (зарплата)
    /// </summary>
    public decimal TotalSalary { get; set; }
    
    /// <summary>
    /// Суммарное кол-во деталей
    /// </summary>
    public int TotalCabinetPartCount { get; set; }
    
    /// <summary>
    /// Суммарные затраты на детали в рублях
    /// </summary>
    public decimal TotalCabinetPartsCost { get; set; }
    
    /// <summary>
    /// Суммарные затраты
    /// </summary>
    public decimal TotalCost { get; set; }

    public TasksStatistics(Guid taskId, string taskName, TaskStatus status, int estimatedWorkHours, int realWorkedHours, 
        decimal totalSalary, int totalCabinetPartCount, decimal totalCabinetPartsCost, decimal totalCost)
    {
        TaskId = taskId;
        TaskName = taskName;
        Status = status;
        EstimatedWorkHours = estimatedWorkHours;
        RealWorkedHours = realWorkedHours;
        TotalSalary = totalSalary;
        TotalCabinetPartCount = totalCabinetPartCount;
        TotalCabinetPartsCost = totalCabinetPartsCost;
        TotalCost = totalCost;
    }
    
}