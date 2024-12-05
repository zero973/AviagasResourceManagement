using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с деталями шкафа
/// </summary>
public class CabinetPartsRepository : BaseDbEntitiesRepository<CabinetPart, Models.Entities.CabinetPart>
{
    public CabinetPartsRepository(AppDbContext context, IMapper mapper, ILogger<CabinetPartsRepository> logger)
        : base(context, mapper, logger)
    {

    }
}