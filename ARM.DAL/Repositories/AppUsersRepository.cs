using ARM.Core.Extensions;
using ARM.Core.Models.UI;
using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями
/// </summary>
public class AppUsersRepository : BaseDbActualEntitiesRepository<AppUser, Models.Entities.AppUser>
{
    
    public AppUsersRepository(AppDbContext context, IMapper mapper, ILogger<AppUsersRepository> logger)
        : base(context, mapper, logger)
    {

    }

    public override async Task<Result<AppUser>> Get(Guid id)
    {
        try
        {
            var result = await _context.Set<Models.Entities.AppUser>()
                .Include(x => x.Employee)
                .SingleAsync(x => x.Id == id);
            return new Result<AppUser>(true, _mapper.Map<AppUser>(result));
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при получении объекта по Id");
            return new Result<AppUser>("Произошла ошибка при попытке получить объект по Id");
        }
    }

    public override async Task<Result<List<AppUser>>> GetAll(BaseListParams baseParams)
    {
        var result = _context.Set<Models.Entities.AppUser>().AsQueryable();

        try
        {
            if (baseParams.Filters?.Any() ?? false)
                result = result.WithFilters(baseParams.Filters);
            result = result.WithOrdering(baseParams).WithPagination(baseParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при применении фильтров, сортировки и пагинации");
            return new Result<List<AppUser>>("Произошла ошибка при применении фильтров, сортировки и пагинации");
        }

        try
        {
            return new Result<List<AppUser>>(true, await _mapper.ProjectTo<AppUser>(result).ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке данных");
            return new Result<List<AppUser>>("Произошла ошибка при загрузке данных");
        }
    }
    
}