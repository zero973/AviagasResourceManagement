using ARM.Core.Models.Security;
using ARM.Core.Models.UI;
using FluentResults;
using MediatR;

namespace ARM.Core.Commands.Requests.Security;

public record LogInRequest(LogInCredentials Credentials, string DeviceId) : IRequest<Result<TokensPair>>;

public record SignUpRequest(SignUpCredentials Credentials, string DeviceId) : IRequest<Result<TokensPair>>;

public record RefreshRequest(TokensPair Pair, string DeviceId) : IRequest<Result<TokensPair>>;