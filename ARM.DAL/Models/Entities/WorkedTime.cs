﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Отработанное время сотрудника
/// </summary>
public class WorkedTime : BaseActualEntity
{
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public AppUser User { get; set; }
    
    public Guid UserId { get; set; }
    
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

    public WorkedTime(Guid userId, Guid taskId, DateTime date, int hours, bool isOverwork)
    {
        UserId = userId;
        TaskId = taskId;
        Date = date;
        Hours = hours;
        IsOverwork = isOverwork;
    }
    
}