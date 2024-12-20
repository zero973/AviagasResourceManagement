using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using ARM.DAL.ApplicationContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Базовая реализация репозитория для типов <see cref="IActualEntity"/>
/// </summary>
/// <typeparam name="T">Базовый тип</typeparam>
/// <typeparam name="U">Тип в БД</typeparam>
public abstract class BaseDbActualEntitiesRepository<T, U> : BaseDbEntitiesRepository<T, U>, IDbActualEntitiesRepository<T>
    where T : class, IActualEntity
    where U : class, IActualEntity
{

    protected readonly AppDbContext _context;
    protected readonly IMapper _mapper;
    protected readonly ILogger<BaseDbActualEntitiesRepository<T, U>> _logger;

    protected BaseDbActualEntitiesRepository(AppDbContext context, IMapper mapper, 
        ILogger<BaseDbActualEntitiesRepository<T, U>> logger) 
        : base(context, mapper, logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task<Result<object>> Remove(Guid id, Guid userId)
    {
        try
        {
            await _context.Set<U>().Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                    .SetProperty(p => p.DeleteDate, _ => DateTime.Now)
                    .SetProperty(p => p.DeletedUserId, _ => userId));

            return new Result<object>(true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении сущности");
            return new Result<object>("Произошла ошибка при удалении сущности");
        }
    }
    
}