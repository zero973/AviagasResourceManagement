using ARM.Core.Extensions;
using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;
using ARM.DAL.ApplicationContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmployeeSalary = ARM.DAL.Models.Entities.EmployeeSalary;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с сотрудниками
/// </summary>
public class EmployeesRepository : BaseDbActualEntitiesRepository<Employee, Models.Entities.Employee>
{
    
    public EmployeesRepository(AppDbContext context, IMapper mapper, ILogger<EmployeesRepository> logger)
        : base(context, mapper, logger)
    {

    }

    public override async Task<Result<Employee>> Get(Guid id)
    {
        try
        {
            var result = await _context.Employees
                .Include(x => x.Salary)
                .SingleAsync(x => x.Id == id);
            return new Result<Employee>(true, _mapper.Map<Employee>(result));
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при получении объекта по Id");
            return new Result<Employee>("Произошла ошибка при попытке получить объект по Id");
        }
    }

    public override async Task<Result<List<Employee>>> GetAll(BaseListParams baseParams)
    {
        var result = _context.Employees
            .Include(x => x.Salary)
            .AsQueryable();

        try
        {
            if (baseParams.Filters?.Any() ?? false)
                result = result.WithFilters(baseParams.Filters);
            result = result.WithOrdering(baseParams).WithPagination(baseParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при применении фильтров, сортировки и пагинации");
            return new Result<List<Employee>>("Произошла ошибка при применении фильтров, сортировки и пагинации");
        }

        try
        {
            return new Result<List<Employee>>(true, await _mapper.ProjectTo<Employee>(result).ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке данных");
            return new Result<List<Employee>>("Произошла ошибка при загрузке данных");
        }
    }

    public override async Task<Result<Employee>> Add(Employee newEntity)
    {
        var entityForSave = _mapper.Map<Models.Entities.Employee>(newEntity);
        var salary = new EmployeeSalary(newEntity.Id, 
            newEntity.SalaryForOneHour, DateTime.Now, DateTime.MaxValue);

        try
        {
            var savedEntity = await _context.Employees.AddAsync(entityForSave);
            var savedSalary = await AddSalary(salary, newEntity);

            await SaveChanges();
            
            savedEntity.Entity.Salary = savedSalary;
            
            return new Result<Employee>(true, _mapper.Map<Employee>(savedEntity.Entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении сущности");
            return new Result<Employee>("Произошла ошибка при добавлении сущности");
        }
    }

    public override async Task<Result<Employee>> Update(Employee entity)
    {
        try
        {
            var entityForSave = _mapper.Map<Models.Entities.Employee>(entity);
            _context.Employees.Update(entityForSave);
            
            var newSalary = new EmployeeSalary(entity.Id, 
                entity.SalaryForOneHour, DateTime.Now, DateTime.MaxValue);

            var curSalary = await _context.EmployeeSalaries
                .SingleAsync(x => x.EmployeeId == entity.Id && x.IsActual);

            if (newSalary.SalaryForOneHour != curSalary.SalaryForOneHour)
            {
                await DeleteSalary(curSalary, entity);
                curSalary = await AddSalary(newSalary, entity);
            }

            await SaveChanges();
            
            entityForSave.Salary = curSalary;
            
            return new Result<Employee>(true, _mapper.Map<Employee>(entityForSave));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при изменении сущности");
            return new Result<Employee>("Произошла ошибка при изменении сущности");
        }
    }

    public override async Task<Result<Employee>> Remove(Employee entity)
    {
        try
        {
            entity.IsActual = false;

            await _context.Employees.Where(x => x.Id == entity.Id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                    .SetProperty(p => p.DeleteDate, e => e.DeleteDate)
                    .SetProperty(p => p.DeletedUserId, e => e.DeletedUserId));
            
            var curSalary = await _context.EmployeeSalaries
                .SingleAsync(x => x.EmployeeId == entity.Id && x.IsActual);

            await DeleteSalary(curSalary, entity);

            return new Result<Employee>(true, entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении сущности");
            return new Result<Employee>("Произошла ошибка при удалении сущности");
        }
    }

    private async Task<EmployeeSalary> AddSalary(EmployeeSalary salary, Employee employee)
    {
        salary.CreatedUserId = employee.CreatedUserId;
        
        return (await _context.EmployeeSalaries.AddAsync(salary)).Entity;
    }

    private async Task DeleteSalary(EmployeeSalary salary, Employee employee)
    {
        salary.IsActual = false;
        salary.End = DateTime.Now;
        salary.UpdateDate = DateTime.Now;
        salary.UpdatedUserId = employee.UpdatedUserId;
        salary.DeleteDate = DateTime.Now;
        salary.DeletedUserId = employee.DeletedUserId;
        
        await _context.EmployeeSalaries.Where(x => x.Id == salary.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                .SetProperty(p => p.End, e => e.End)
                .SetProperty(p => p.UpdateDate, e => e.UpdateDate)
                .SetProperty(p => p.UpdatedUserId, e => e.UpdatedUserId)
                .SetProperty(p => p.DeleteDate, e => e.DeleteDate)
                .SetProperty(p => p.DeletedUserId, e => e.DeletedUserId));
    }
    
}