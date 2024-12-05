using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы со шкафами
/// </summary>
public class CabinetsRepository : BaseDbEntitiesRepository<Cabinet, Models.Entities.Cabinet>
{
    public CabinetsRepository(AppDbContext context, IMapper mapper, ILogger<CabinetsRepository> logger)
        : base(context, mapper, logger)
    {

    }
}