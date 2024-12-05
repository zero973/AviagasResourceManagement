namespace ARM.DAL.Models.Entities;

/// <summary>
/// Кол-во деталей, которое привязано к задаче
/// </summary>
public class CabinetPartCounts : BaseActualEntity
{
    
    /// <summary>
    /// Деталь шкафа
    /// </summary>
    public CabinetPart CabinetPart { get; set; }
    
    /// <summary>
    /// Деталь шкафа
    /// </summary>
    public Guid CabinetPartId { get; set; }
    
    /// <summary>
    /// Задача, для которой предназначено это кол-во деталей
    /// </summary>
    public SystemTask LinkedTask { get; set; }
    
    /// <summary>
    /// Задача, для которой предназначено это кол-во деталей
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// Кол-во деталей
    /// </summary>
    public int Count { get; set; }

    public CabinetPartCounts()
    {
        
    }

    public CabinetPartCounts(Guid cabinetPartId, Guid taskId, int count)
    {
        CabinetPartId = cabinetPartId;
        TaskId = taskId;
        Count = count;
    }
    
}