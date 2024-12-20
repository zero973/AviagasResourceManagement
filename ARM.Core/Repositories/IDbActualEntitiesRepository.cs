using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;

namespace ARM.Core.Repositories;

public interface IDbActualEntitiesRepository<T> : IDbEntitiesRepository<T>
    where T : class, IActualEntity
{
    
    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="Id">Id сущности</param>
    /// <param name="userId">Id пользователя, который удалил сущность</param>
    Task<Result<object>> Remove(Guid id, Guid userId);
    
}