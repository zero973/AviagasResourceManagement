namespace ARM.Core.Models.Entities;

/// <summary>
/// Деталь шкафа
/// </summary>
public class CabinetPart : BaseEntity
{
    
    /// <summary>
    /// Название детали
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Цена детали
    /// </summary>
    public decimal Cost { get; set; }
    
}