using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

public class CabinetPartCountsRepository : BaseDbEntitiesRepository<CabinetPartCounts, Models.Entities.CabinetPartCounts>
{
    public CabinetPartCountsRepository(AppDbContext context, IMapper mapper, ILogger<CabinetPartCountsRepository> logger)
        : base(context, mapper, logger)
    {

    }
}