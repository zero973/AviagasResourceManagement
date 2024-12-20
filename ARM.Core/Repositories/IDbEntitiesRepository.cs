using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;

namespace ARM.Core.Repositories;

public interface IDbEntitiesRepository<T> where T : class, IEntity
{
    
    /// <summary>
    /// Получить объект по <see cref="IEntity.Id"/>
    /// </summary>
    Task<Result<T>> Get(Guid id);

    /// <summary>
    /// Получить все сущности с фильтрами <paramref name="filters"/>
    /// </summary>
    Task<Result<List<T>>> GetAll(BaseListParams baseParams);

    /// <summary>
    /// Добавить сущность
    /// </summary>
    Task<Result<T>> Add(T newEntity);

    /// <summary>
    /// Добавить диапазон сущностей
    /// </summary>
    Task AddRange(IEnumerable<T> newEntities);

    /// <summary>
    /// Изменить сущность
    /// </summary>
    Task<Result<T>> Update(T entity);

    /// <summary>
    /// Изменить диапазон сущностей
    /// </summary>
    Task UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    Task<Result<object>> Remove(Guid Id);

    /// <summary>
    /// Удалить диапазон сущностей
    /// </summary>
    Task RemoveRange(IEnumerable<T> entities);
    
}