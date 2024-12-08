namespace ARM.Core.Models.Entities;

/// <summary>
/// Комментарий в задаче
/// </summary>
public class Comment : BaseActualEntity
{
    
    /// <summary>
    /// Автор комментария
    /// </summary>
    public required Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Задача, в которй был оставлен коментарий
    /// </summary>
    public required Guid TaskId { get; set; }
    
    /// <summary>
    /// Текст комментария
    /// </summary>
    public required string Text { get; set; }
    
}