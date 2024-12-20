﻿using ARM.Core.Commands.Requests.Entities.ActualEntities;
using ARM.Core.Helpers;
using ARM.Core.Models.Entities;
using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using ARM.Core.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ARM.Core.Services.Security.Impl;

public class AuthorizationService : IAuthorizationService
{

    private readonly IJwtService _jwtService;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IAuthorizationRepository _authorizationRepository;
    private readonly ISender _sender;
    private readonly TokenValidationParameters _tokenValidationParameters;
    
    public AuthorizationService(IJwtService jwtService, IRefreshTokensRepository refreshTokensRepository, 
        ISender sender, TokenValidationParameters tokenValidationParameters, 
        IAuthorizationRepository authorizationRepository)
    {
        _jwtService = jwtService;
        _sender = sender;
        _refreshTokensRepository = refreshTokensRepository;
        _tokenValidationParameters = tokenValidationParameters;
        _authorizationRepository = authorizationRepository;
    }
    
    public async Task<Result<TokensPair>> LogIn(LogInCredentials credentials, string deviceId)
    {
        var passwordHash = Encryptor.EncryptString(credentials.Password);

        var userResult = await _authorizationRepository.GetUserByLoginAndPassword(credentials.Login, passwordHash);

        if (!userResult.IsSuccess)
            return new Result<TokensPair>("Пользователь с таким логином и паролем не найден");
        
        var user = userResult.Data;

        await _refreshTokensRepository.RevokeTokenIfExists(user.Id, deviceId);

        var token = await _jwtService.GenerateTokenForUser(user, deviceId);
        return new Result<TokensPair>(true, token);
    }

    public async Task<Result<TokensPair>> SignUp(SignUpCredentials credentials, string deviceId)
    {
        throw new NotImplementedException();
        
        var newUser = new EmployeeAccount()
        {
            Login = credentials.Login,
            PasswordHash = Encryptor.EncryptString(credentials.Password)
        };

        var token = await _jwtService.GenerateTokenForUser(newUser, deviceId);
        return new Result<TokensPair>(true, token);
    }

    public async Task<Result<TokensPair>> Refresh(TokensPair pair, string deviceId)
    {
        var validatedToken = JwtUtils.GetPrincipalFromToken(pair.AccessToken, _tokenValidationParameters)!;
        var userId = Guid.Parse(validatedToken.Claims.Single(x => x.Type == "id").Value);

        var userResult = await _sender.Send(new GetActualDataRequest<EmployeeAccount>(userId));

        if (!userResult.IsSuccess)
            return new Result<TokensPair>(userResult.Message);

        var storedRefreshToken = (await _refreshTokensRepository.GetToken(pair.RefreshToken))!;
        if (storedRefreshToken.IsRevoked)
            return new Result<TokensPair>("Токен отозван");

        if (storedRefreshToken.IsUsed)
        {
            await _refreshTokensRepository.RevokeTokenIfExists(userId, deviceId);
            return new Result<TokensPair>("Токен уже использован");
        }

        await _refreshTokensRepository.UseToken(pair.RefreshToken);
        var token = await _jwtService.GenerateTokenForUser(userResult.Data, deviceId);
        
        return new Result<TokensPair>(true, token);
    }
    
}