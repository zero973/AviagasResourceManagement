namespace ARM.Core.Models.Entities.Intf;

/// <summary>
/// Сущность с переключаемой актуальностью
/// </summary>
public interface IActualEntity : IEntity
{
    
    /// <summary>
    /// Актуальна ли сущность
    /// </summary>
    bool IsActual { get; set; }

    /// <summary>
    /// Пользователь добавивший сущность
    /// </summary>
    Guid? CreatedUserId { get; set; }

    /// <summary>
    /// Дата создания сущности
    /// </summary>
    DateTime CreateDate { get; set; }

    /// <summary>
    /// Пользователь изменивший сущность
    /// </summary>
    Guid? UpdatedUserId { get; set; }

    /// <summary>
    /// Дата изменения сущности
    /// </summary>
    DateTime? UpdateDate { get; set; }

    /// <summary>
    /// Пользователь удаливший сущность
    /// </summary>
    Guid? DeletedUserId { get; set; }

    /// <summary>
    /// Дата удаления сущности
    /// </summary>
    DateTime? DeleteDate { get; set; }
    
}