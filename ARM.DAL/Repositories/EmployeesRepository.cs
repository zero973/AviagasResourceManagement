using ARM.Core.Extensions;
using ARM.Core.Models.Entities;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using ARM.DAL.ApplicationContexts;
using ARM.DAL.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmployeeSalary = ARM.DAL.Models.Entities.EmployeeSalary;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с сотрудниками
/// </summary>
public class EmployeesRepository : BaseDbActualEntitiesRepository<EmployeeAccount, Employee>, IAuthorizationRepository
{
    
    public EmployeesRepository(AppDbContext context, IMapper mapper, ILogger<EmployeesRepository> logger)
        : base(context, mapper, logger)
    {

    }
    
    public async Task<Result<EmployeeAccount>> GetUserByLoginAndPassword(string login, string passwordHash)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Login == login && x.PasswordHash == passwordHash);

            if (user == null)
                return new Result<EmployeeAccount>("Не удалось найти пользователя с таким логином и паролем");

            return await Get(user.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении объекта по Id");
            return new Result<EmployeeAccount>("Произошла ошибка при попытке получить объект по Id");
        }
    }

    public override async Task<Result<EmployeeAccount>> Get(Guid id)
    {
        try
        {
            var employee = await _context.Employees
                .Include(x => x.Salary)
                .SingleAsync(x => x.Id == id);

            var user = await _context.Users
                .SingleAsync(x => x.Id == id);

            var result = new EmployeeAccount(user.Login, user.Role, 
                $"{employee.LastName} {employee.FirstName} {employee.Patronymic}", employee.Birthday, 
                employee.Passport, employee.Salary!.SalaryForOneHour, employee.PhotoUrl)
                {
                    Id = employee.Id,
                    CreateDate = employee.CreateDate, 
                    CreatedUserId = employee.CreatedUserId,
                    UpdateDate = employee.UpdateDate, 
                    UpdatedUserId = employee.UpdatedUserId,
                    DeleteDate = employee.DeleteDate, 
                    DeletedUserId = employee.DeletedUserId
                };
            
            return new Result<EmployeeAccount>(true, result);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при получении объекта по Id");
            return new Result<EmployeeAccount>("Произошла ошибка при попытке получить объект по Id");
        }
    }

    public override async Task<Result<List<EmployeeAccount>>> GetAll(BaseListParams baseParams)
    {
        var employees = _context.Employees
            .Include(x => x.Salary)
            .AsQueryable();

        try
        {
            if (baseParams.Filters?.Any() ?? false)
                employees = employees.WithFilters(baseParams.Filters);
            employees = employees.WithOrdering(baseParams).WithPagination(baseParams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при применении фильтров, сортировки и пагинации");
            return new Result<List<EmployeeAccount>>("Произошла ошибка при применении фильтров, сортировки и пагинации");
        }

        try
        {
            var result = employees.Join(_context.Users, e => e.Id, u => u.Id, 
                (e, u) => new EmployeeAccount(u.Login, u.Role, $"{e.LastName} {e.FirstName} {e.Patronymic}", 
                    e.Birthday, e.Passport, e.Salary!.SalaryForOneHour, e.PhotoUrl)
                {
                    Id = e.Id,
                    CreateDate = e.CreateDate, 
                    CreatedUserId = e.CreatedUserId,
                    UpdateDate = e.UpdateDate, 
                    UpdatedUserId = e.UpdatedUserId,
                    DeleteDate = e.DeleteDate, 
                    DeletedUserId = e.DeletedUserId
                });
            
            return new Result<List<EmployeeAccount>>(true, await result.ToListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке данных");
            return new Result<List<EmployeeAccount>>("Произошла ошибка при загрузке данных");
        }
    }

    public override async Task<Result<EmployeeAccount>> Add(EmployeeAccount newEntity)
    {
        var entityForSave = _mapper.Map<Employee>(newEntity);
        var salary = new EmployeeSalary(newEntity.Id, newEntity.SalaryForOneHour, DateTime.Now, DateTime.MaxValue);

        try
        {
            var savedEntity = await _context.Employees.AddAsync(entityForSave);
            var savedSalary = await AddSalary(salary, newEntity);
            await _context.Users.AddAsync(GetAppUser(newEntity));

            await SaveChanges();
            
            savedEntity.Entity.Salary = savedSalary;
            
            return new Result<EmployeeAccount>(true, _mapper.Map<EmployeeAccount>(savedEntity.Entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении сущности");
            return new Result<EmployeeAccount>("Произошла ошибка при добавлении сущности");
        }
    }

    public override async Task<Result<EmployeeAccount>> Update(EmployeeAccount entity)
    {
        try
        {
            var entityForSave = _mapper.Map<Employee>(entity);
            _context.Employees.Update(entityForSave);
            _context.Users.Update(GetAppUser(entity));
            
            var newSalary = new EmployeeSalary(entity.Id, 
                entity.SalaryForOneHour, DateTime.Now, DateTime.MaxValue);

            var curSalary = await _context.EmployeeSalaries
                .SingleAsync(x => x.EmployeeId == entity.Id && x.IsActual);

            if (newSalary.SalaryForOneHour != curSalary.SalaryForOneHour)
            {
                await DeleteSalary(curSalary, entityForSave.UpdatedUserId!.Value);
                curSalary = await AddSalary(newSalary, entity);
            }

            await SaveChanges();
            
            entityForSave.Salary = curSalary;
            
            return new Result<EmployeeAccount>(true, _mapper.Map<EmployeeAccount>(entityForSave));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при изменении сущности");
            return new Result<EmployeeAccount>("Произошла ошибка при изменении сущности");
        }
    }

    public override async Task<Result<object>> Remove(Guid id, Guid userId)
    {
        try
        {
            await _context.Employees.Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                    .SetProperty(p => p.DeleteDate, _ => DateTime.Now)
                    .SetProperty(p => p.DeletedUserId, _ => userId));

            await _context.Users.Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                    .SetProperty(p => p.DeleteDate, _ => DateTime.Now)
                    .SetProperty(p => p.DeletedUserId, _ => userId));
            
            var curSalary = await _context.EmployeeSalaries
                .SingleAsync(x => x.EmployeeId == id && x.IsActual);

            await DeleteSalary(curSalary, userId);

            return new Result<object>(true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении сущности");
            return new Result<object>("Произошла ошибка при удалении сущности");
        }
    }

    private async Task<EmployeeSalary> AddSalary(EmployeeSalary salary, EmployeeAccount employee)
    {
        salary.CreatedUserId = employee.CreatedUserId;
        
        return (await _context.EmployeeSalaries.AddAsync(salary)).Entity;
    }

    private async Task DeleteSalary(EmployeeSalary salary, Guid userId)
    {
        await _context.EmployeeSalaries.Where(x => x.Id == salary.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsActual, _ => false)
                .SetProperty(p => p.End, _ => DateTime.Now)
                .SetProperty(p => p.UpdateDate, _ => DateTime.Now)
                .SetProperty(p => p.UpdatedUserId, _ => userId)
                .SetProperty(p => p.DeleteDate, _ => DateTime.Now)
                .SetProperty(p => p.DeletedUserId, _ => userId));
    }

    private AppUser GetAppUser(EmployeeAccount user) 
        => new AppUser(user.Login, user.PasswordHash, user.Role, user.Id) 
        { 
            Id = user.Id, 
            CreateDate = user.CreateDate, 
            CreatedUserId = user.CreatedUserId,
            UpdateDate = user.UpdateDate, 
            UpdatedUserId = user.UpdatedUserId,
            DeleteDate = user.DeleteDate, 
            DeletedUserId = user.DeletedUserId 
        };
    
}