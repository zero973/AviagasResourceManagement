using System.ComponentModel.DataAnnotations;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Деталь шкафа
/// </summary>
public class CabinetPart : BaseEntity
{
    
    /// <summary>
    /// Название детали
    /// </summary>
    [MaxLength(200)]
    public string Name { get; set; }
    
    /// <summary>
    /// Цена детали
    /// </summary>
    public decimal Cost { get; set; }
    
    public ICollection<CabinetPartCounts> CabinetPartCounts { get; set; }

    public CabinetPart()
    {
        
    }

    public CabinetPart(string name, decimal cost)
    {
        Name = name;
        Cost = cost;
    }
    
}