using System.ComponentModel.DataAnnotations.Schema;
using ARM.DAL.Constants;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Комментарий в задаче
/// </summary>
[Table(DbConstants.CommentTableName, Schema = DbConstants.DataSchema)]
public class Comment : BaseActualEntity
{
    
    /// <summary>
    /// Автор комментария
    /// </summary>
    public Employee Employee { get; set; }
    
    /// <summary>
    /// Автор комментария
    /// </summary>
    public Guid EmployeeId { get; set; }
    
    /// <summary>
    /// Задача, в которой был оставлен коментарий
    /// </summary>
    public SystemTask LinkedTask { get; set; }
    
    /// <summary>
    /// Задача, в которой был оставлен коментарий
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// Текст комментария
    /// </summary>
    [Column(TypeName = "text")]
    public string Text { get; set; }

    public Comment()
    {
        
    }

    public Comment(Guid employeeId, Guid taskId, string text)
    {
        EmployeeId = employeeId;
        TaskId = taskId;
        Text = text;
    }
    
}