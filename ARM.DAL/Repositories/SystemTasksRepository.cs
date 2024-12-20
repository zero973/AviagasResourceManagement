using ARM.Core.Enums;
using ARM.Core.Extensions;
using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using AutoMapper;
using ARM.DAL.ApplicationContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

public class SystemTasksRepository : BaseDbActualEntitiesRepository<SystemTask, Models.Entities.SystemTask>
{

    private readonly IDbEntitiesRepository<TaskEmployee> _taskEmployeesRepository;
    private readonly IDbActualEntitiesRepository<WorkedTime> _workedHoursRepository;
    private readonly IDbActualEntitiesRepository<Comment> _commentsRepository;
    private readonly IDbEntitiesRepository<CabinetPartCounts> _cabinetPartCountsRepository;
    private readonly IDbActualEntitiesRepository<EmployeeAccount> _employeesRepository;
    private readonly IDbEntitiesRepository<CabinetPart> _cabinetPartsRepository;
    
    public SystemTasksRepository(AppDbContext context, IMapper mapper, ILogger<SystemTasksRepository> logger, 
            IDbActualEntitiesRepository<WorkedTime> workedHoursRepository, 
            IDbActualEntitiesRepository<Comment> commentsRepository, 
            IDbEntitiesRepository<TaskEmployee> taskEmployeesRepository, 
            IDbEntitiesRepository<CabinetPartCounts> cabinetPartCountsRepository, 
            IDbActualEntitiesRepository<EmployeeAccount> employeesRepository, 
            IDbEntitiesRepository<CabinetPart> cabinetPartsRepository)
        : base(context, mapper, logger)
    {
        _workedHoursRepository = workedHoursRepository;
        _commentsRepository = commentsRepository;
        _taskEmployeesRepository = taskEmployeesRepository;
        _cabinetPartCountsRepository = cabinetPartCountsRepository;
        _employeesRepository = employeesRepository;
        _cabinetPartsRepository = cabinetPartsRepository;
    }
    
    public override async Task<Result<SystemTask>> Get(Guid id)
    {
        try
        {
            var taskDb = await _context.Taks
                .Include(x => x.Cabinet)
                .Include(x => x.CurrentPerformer)
                .SingleAsync(x => x.Id == id);
            var result = _mapper.Map<SystemTask>(taskDb);

            var taskIdFilter = new BaseListParams()
                .WithPagination()
                .WithFilter(nameof(Models.Entities.TaskEmployee.TaskId), ComplexFilterOperators.Equals, id);
            
            var employeesIds = (await _taskEmployeesRepository.GetAll(taskIdFilter)).Data
                .Select(x => (object)x.EmployeeId)
                .ToArray();
            
            var prms = new BaseListParams()
                .WithActualPagination()
                .WithFilter(nameof(Models.Entities.Employee.Id), ComplexFilterOperators.In, employeesIds);
            
            var employees = (await _employeesRepository.GetAll(prms)).Data;

            taskIdFilter = taskIdFilter.WithActualPagination();
            
            var workedHours = (await _workedHoursRepository.GetAll(taskIdFilter)).Data.Sum(x => x.Hours);
            
            var comments = (await _commentsRepository.GetAll(taskIdFilter)).Data;
            
            var cabinetPartCountsIds = (await _cabinetPartCountsRepository.GetAll(taskIdFilter)).Data
                .ToDictionary(x => x.CabinetPartId, x => x);
            
            prms = new BaseListParams()
                .WithPagination()
                .WithFilter(nameof(Models.Entities.CabinetPart.Id), ComplexFilterOperators.In, 
                    cabinetPartCountsIds.Select(x => (object)x.Key).ToArray());
            
            var cabinetPartCounts = (await _cabinetPartsRepository.GetAll(prms)).Data
                .Select(x => new CabinetPartCounts()
                {
                    Id = cabinetPartCountsIds[x.Id].Id,
                    CabinetPart = x, 
                    CabinetPartId = x.Id, 
                    Count = cabinetPartCountsIds[x.Id].Count, 
                    TaskId = id
                }).ToList();

            result.Employees = employees;
            result.RealWorkedHours = workedHours;
            result.Comments = comments;
            result.CabinetParts = cabinetPartCounts;
            
            return new Result<SystemTask>(true, result);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при получении объекта по Id");
            return new Result<SystemTask>("Произошла ошибка при попытке получить объект по Id");
        }
    }

    public override async Task<Result<List<SystemTask>>> GetAll(BaseListParams baseParams)
    {
        var result = _context.Set<Models.Entities.SystemTask>().AsQueryable();

        try
        {
            if (baseParams.Filters?.Any() ?? false)
                result = result.WithFilters(baseParams.Filters);
            result = result.WithOrdering(baseParams).WithPagination(baseParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при применении фильтров, сортировки и пагинации");
            return new Result<List<SystemTask>>("Произошла ошибка при применении фильтров, сортировки и пагинации");
        }

        try
        {
            return new Result<List<SystemTask>>(true, await _mapper.ProjectTo<SystemTask>(result).ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке данных");
            return new Result<List<SystemTask>>("Произошла ошибка при загрузке данных");
        }
    }
    
}