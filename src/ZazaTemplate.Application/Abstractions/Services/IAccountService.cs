using ZazaTemplate.Contracts.ApiContracts;
using ZazaTemplate.Domain;
using Microsoft.AspNetCore.Http;
using ResultSharp.Core;

namespace ZazaTemplate.Application.Abstractions.Services;

public interface IAccountService
{
    Task<Result<User>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<User>> GetCurrentUserAsync(HttpRequest request, CancellationToken cancellationToken);
    Task<Result<User>> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Result<User>> RegisterAsync(RegisterRequest request, HttpResponse response, CancellationToken cancellationToken);
    Task<Result> LoginAsync(LoginRequest request, HttpResponse response, CancellationToken cancellationToken);
    Task<Result> LogoutAsync(HttpResponse response, CancellationToken cancellationToken);
    Task<Result> RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
}
