using ARM.DAL.Models.Security;
using ARM.Core.Repositories;
using ARM.DAL.ApplicationContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARM.DAL.Repositories;

/// <summary>
/// Репозиторий для работы с RefreshTokens
/// </summary>
public class RefreshTokensRepository : IRefreshTokensRepository
{
    
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RefreshTokensRepository> _logger;

    public RefreshTokensRepository(AppDbContext context, IMapper mapper, ILogger<RefreshTokensRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Core.Models.Security.RefreshToken> GetToken(string token)
    {
        try
        {
            var result = await _context.RefreshTokens
                .SingleAsync(x => x.Token == token);
            return _mapper.Map<Core.Models.Security.RefreshToken>(result);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Ошибка при получении RefreshToken");
            throw;
        }
    }

    public async Task CreateToken(Core.Models.Security.RefreshToken token)
    {
        var tokenForSave = _mapper.Map<RefreshToken>(token);
        _context.RefreshTokens.Add(tokenForSave);
        await _context.SaveChangesAsync();
    }

    public async Task UseToken(string token)
    {
        await _context.RefreshTokens
            .Where(x => x.Token == token)
            .ExecuteUpdateAsync(x => x.SetProperty(
                e => e.IsUsed,
                _ => true));
    }

    public async Task RevokeTokenIfExists(Guid userId, string deviceId)
    {
        await _context.RefreshTokens
            .Where(x => x.UserId == userId && x.DeviceId == deviceId && !x.IsUsed && !x.IsRevoked)
            .ExecuteUpdateAsync(x => x.SetProperty(
                e => e.IsRevoked,
                _ => true));
    }
    
}