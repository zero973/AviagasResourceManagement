namespace ARM.Core.Models.UI;

/// <summary>
/// Электрощит управления станцией и их количество
/// </summary>
public class CabinetsCounts
{
    
    /// <summary>
    /// Электрощит управления станцией
    /// </summary>
    public Guid CabinetId { get; set; }
    
    /// <summary>
    /// Кол-во шкафов
    /// </summary>
    public int Count { get; set; }
    
}