namespace ARM.Core.Models.Entities.Intf;

/// <summary>
/// Сущность с идентификатором
/// </summary>
public interface IEntity : IEquatable<IEntity>
{
    
    /// <summary>
    /// Id сущности
    /// </summary>
    Guid Id { get; set; }
    
}