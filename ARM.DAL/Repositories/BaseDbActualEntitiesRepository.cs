using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
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
public abstract class BaseDbActualEntitiesRepository<T, U> : BaseDbEntitiesRepository<T, U>
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

    public override async Task<Result<T>> Remove(T entity)
    {
        try
        {
            entity.IsActual = false;

            await _context.Set<U>().Where(x => x.Id == entity.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                    .SetProperty(p => p.DeleteDate, e => e.DeleteDate)
                    .SetProperty(p => p.DeletedUserId, e => e.DeletedUserId));

            return new Result<T>(true, entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении сущности");
            return new Result<T>("Произошла ошибка при удалении сущности");
        }
    }
    
}