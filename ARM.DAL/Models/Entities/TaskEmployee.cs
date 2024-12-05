﻿namespace ARM.DAL.Models.Entities;

/// <summary>
/// Сотрудник, прикреплённый к задаче
/// </summary>
public class TaskEmployee : BaseEntity
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
    /// Задача
    /// </summary>
    public SystemTask LinkedTask { get; set; }
    
    /// <summary>
    /// Задача
    /// </summary>
    public Guid TaskId { get; set; }

    public TaskEmployee()
    {
        
    }
    
    public TaskEmployee(Guid employeeId, Guid taskId)
    {
        EmployeeId = employeeId;
        TaskId = taskId;
    }
    
}