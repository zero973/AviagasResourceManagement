using System.ComponentModel.DataAnnotations;

namespace ARM.DAL.Models.Entities;

/// <summary>
/// Шкаф
/// </summary>
public class Cabinet : BaseEntity
{
    
    /// <summary>
    /// Сокращённое название шкафа
    /// </summary>
    [MaxLength(200)]
    public string Name { get; set; }
    
    /// <summary>
    /// Полное название шкафа
    /// </summary>
    [MaxLength(200)]
    public string Fullname { get; set; }

    public Cabinet()
    {
        
    }

    public Cabinet(string name, string fullname)
    {
        Name = name;
        Fullname = fullname;
    }
    
}