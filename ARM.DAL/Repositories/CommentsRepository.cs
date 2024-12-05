using AutoMapper;
using ARM.DAL.ApplicationContexts;
using ARM.Core.Models.Entities;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с комментариями
/// </summary>
public class CommentsRepository : BaseDbActualEntitiesRepository<Comment, Models.Entities.Comment>
{
    public CommentsRepository(AppDbContext context, IMapper mapper, ILogger<CommentsRepository> logger)
        : base(context, mapper, logger)
    {

    }
}