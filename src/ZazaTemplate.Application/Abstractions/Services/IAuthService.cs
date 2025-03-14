using ZazaTemplate.Domain;
using Microsoft.AspNetCore.Http;
using ResultSharp.Core;

namespace ZazaTemplate.Application.Abstractions.Services;

public interface IAuthService
{
    Result<string> GenerateAccessToken(User user, Role role);
    Task<Result<string>> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken);
    Task<Result<(string accessToken, string refreshToken)>> GenerateAccessRefreshPairAsync(User user, Role role, CancellationToken cancellationToken);
    Task<Result> GenerateAndSetTokensAsync(User user, Role role, HttpResponse response, CancellationToken cancellationToken);
    Result<Guid> GetUserIdFromAccessToken(HttpRequest request);
    Result<Role> GetRoleFromAccessToken(HttpRequest request);
    Result ValidateToken(HttpRequest request, string header);
    Task<Result> ClearTokensAsync(HttpResponse response, CancellationToken cancellationToken);
    Task<Result> RefreshAccessTokenAsync(HttpRequest request, HttpResponse response, User user, CancellationToken cancellationToken);
}

