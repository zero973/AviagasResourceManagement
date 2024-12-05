using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с сотрудниками, прикреплёнными к задаче
/// </summary>
public class TaskEmployeesRepository : BaseDbEntitiesRepository<TaskEmployee, Models.Entities.TaskEmployee>
{
    public TaskEmployeesRepository(AppDbContext context, IMapper mapper, ILogger<TaskEmployeesRepository> logger)
        : base(context, mapper, logger)
    {

    }
}