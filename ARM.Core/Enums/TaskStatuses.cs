namespace ARM.Core.Enums;

/// <summary>
/// Статусы задачи
/// </summary>
public enum TaskStatuses
{
    
    /// <summary>
    /// Новый
    /// </summary>
    New = 0,
    
    /// <summary>
    /// Сбор комплектующих
    /// </summary>
    AssemblingComponents = 1,
    
    /// <summary>
    /// Монтаж
    /// </summary>
    Installation = 2,
    
    /// <summary>
    /// Испытание
    /// </summary>
    Testing = 3,
    
    /// <summary>
    /// Закрыт
    /// </summary>
    Closed = 4
    
}