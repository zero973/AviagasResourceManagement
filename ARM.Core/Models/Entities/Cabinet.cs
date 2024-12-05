namespace ARM.Core.Models.Entities;

/// <summary>
/// Шкаф
/// </summary>
public class Cabinet : BaseEntity
{
    
    /// <summary>
    /// Название шкафа
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Полное название шкафа
    /// </summary>
    public required string Fullname { get; set; }
    
}