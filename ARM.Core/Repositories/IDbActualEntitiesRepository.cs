using ARM.Core.Models.Entities.Intf;
using FluentResults;

namespace ARM.Core.Repositories;

public interface IDbActualEntitiesRepository<T> : IDbEntitiesRepository<T>
    where T : class, IActualEntity
{
    
    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="Id">Id сущности</param>
    /// <param name="userId">Id пользователя, который удалил сущность</param>
    Task<Result> Remove(Guid id, Guid userId);
    
}