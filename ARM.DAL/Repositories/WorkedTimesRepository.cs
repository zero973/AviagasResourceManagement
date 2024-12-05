using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

public class WorkedTimesRepository : BaseDbActualEntitiesRepository<WorkedTime, Models.Entities.WorkedTime>
{
    public WorkedTimesRepository(AppDbContext context, IMapper mapper, ILogger<WorkedTimesRepository> logger)
        : base(context, mapper, logger)
    {

    }
}