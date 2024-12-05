namespace ARM.Core.Models.Entities;

/// <summary>
/// Сотрудник, прикреплённый к задаче
/// </summary>
public class TaskEmployee : BaseEntity
{
    
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Задача
    /// </summary>
    public Guid TaskId { get; set; }
    
}