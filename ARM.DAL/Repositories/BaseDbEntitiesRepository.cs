using System.Data;
using ARM.Core.Extensions;
using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using ARM.DAL.ApplicationContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Базовая реализация репозитория для типов <see cref="IEntity"/>
/// </summary>
/// <typeparam name="T">Базовый тип</typeparam>
/// <typeparam name="U">Тип в БД</typeparam>
public abstract class BaseDbEntitiesRepository<T, U> : IDbEntitiesRepository<T>
    where T : class, IEntity
    where U : class, IEntity
{

    protected readonly AppDbContext _context;
    protected readonly IMapper _mapper;
    protected readonly ILogger<BaseDbEntitiesRepository<T, U>> _logger;

    protected BaseDbEntitiesRepository(AppDbContext context, IMapper mapper, 
        ILogger<BaseDbEntitiesRepository<T, U>> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public virtual async Task<Result<T>> Get(Guid id)
    {
        try
        {
            var result = await _context.Set<U>().SingleAsync(x => x.Id == id);
            return new Result<T>(true, _mapper.Map<T>(result));
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при получении объекта по Id");
            return new Result<T>("Произошла ошибка при попытке получить объект по Id");
        }
    }

    public virtual async Task<Result<List<T>>> GetAll(BaseListParams baseParams)
    {
        var result = _context.Set<U>().AsQueryable();

        try
        {
            if (baseParams.Filters?.Any() ?? false)
                result = result.WithFilters(baseParams.Filters);
            result = result.WithOrdering(baseParams).WithPagination(baseParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при применении фильтров, сортировки и пагинации");
            return new Result<List<T>>("Произошла ошибка при применении фильтров, сортировки и пагинации");
        }

        try
        {
            return new Result<List<T>>(true, await _mapper.ProjectTo<T>(result).ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке данных");
            return new Result<List<T>>("Произошла ошибка при загрузке данных");
        }
    }

    public virtual async Task<Result<T>> Add(T newEntity)
    {
        var entityForSave = _mapper.Map<U>(newEntity);

        try
        {
            var savedEntity = await _context.Set<U>().AddAsync(entityForSave);

            await SaveChanges();
            return new Result<T>(true, _mapper.Map<T>(savedEntity.Entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении сущности");
            return new Result<T>("Произошла ошибка при добавлении сущности");
        }
    }

    public virtual async Task AddRange(IEnumerable<T> newEntities)
    {
        var entitiesForSave = newEntities.Select(_mapper.Map<U>);
        await _context.Set<U>().AddRangeAsync(entitiesForSave);

        await SaveChanges();
    }

    public virtual async Task<Result<T>> Update(T entity)
    {
        try
        {
            var entityForSave = _mapper.Map<U>(entity);
            
            var res = _context
                .ChangeTracker
                .Entries<U>()
                .FirstOrDefault(x => x.State != EntityState.Detached && x.Entity.Id == entity.Id);
            if (res is not null)
                res.State = EntityState.Detached;
            
            _context.Set<U>().Update(entityForSave);

            await SaveChanges();
            return new Result<T>(true, _mapper.Map<T>(entityForSave));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при изменении сущности");
            return new Result<T>("Произошла ошибка при изменении сущности");
        }
    }

    public virtual async Task UpdateRange(IEnumerable<T> entities)
    {
        var entitiesForSave = entities.Select(_mapper.Map<U>);
        _context.Set<U>().UpdateRange(entitiesForSave);

        await SaveChanges();
    }

    public virtual async Task<Result<T>> Remove(T entity)
    {
        try
        {
            var entityForSave = _mapper.Map<U>(entity);
            _context.Set<U>().Remove(entityForSave);

            await SaveChanges();
            return new Result<T>(true, _mapper.Map<T>(entityForSave));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении сущности");
            return new Result<T>("Произошла ошибка при удалении сущности");
        }
    }

    public virtual async Task RemoveRange(IEnumerable<T> entities)
    {
        var entitiesToDelete = entities.Select(_mapper.Map<U>);

        _context.Set<U>().RemoveRange(entitiesToDelete);

        await SaveChanges();
    }

    /// <summary>
    /// Сохраняет изменения в базу данных
    /// </summary>
    /// <returns>Возвращает кол-во добавленных/изменённых строк</returns>
    protected virtual async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }

    protected (IDbConnection connection, IDbTransaction? transaction) GetConnection()
    {
        var connection = _context.Database.GetDbConnection();
        return (connection, _context.Database.CurrentTransaction?.GetDbTransaction());
    }
    
}