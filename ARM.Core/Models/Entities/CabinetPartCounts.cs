namespace ARM.Core.Models.Entities;

/// <summary>
/// Кол-во деталей к шкафу в задаче
/// </summary>
public class CabinetPartCounts : BaseEntity
{
    
    /// <summary>
    /// Деталь шкафа
    /// </summary>
    public Guid CabinetPartId { get; set; }
    
    /// <summary>
    /// Задача, для которой предназначено это кол-во деталей
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// Кол-во деталей
    /// </summary>
    public int Count { get; set; }
    
}