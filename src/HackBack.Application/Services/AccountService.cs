using ZazaTemplate.Application.Abstractions.Auth;
using ZazaTemplate.Application.Abstractions.Data;
using ZazaTemplate.Application.Abstractions.Services;
using ZazaTemplate.Contracts.ApiContracts;
using ZazaTemplate.Domain;
using Microsoft.AspNetCore.Http;
using ResultSharp.Core;
using ResultSharp.Errors;
using ResultSharp.Extensions.FunctionalExtensions.Async;
using ResultSharp.Logging;
using ZazaTemplate.Application.Abstractions.Services;

namespace ZazaTemplate.Application.Services;

public class AccountService(
    IUserRepository studentRepository,
    IAuthService authService,
    IPasswordManager passwordManager) : IAccountService
{
    private readonly IUserRepository studentRepository = studentRepository;
    private readonly IAuthService authService = authService;
    private readonly IPasswordManager passwordManager = passwordManager;

    public async Task<Result<User>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await studentRepository.GetAsync(id, cancellationToken);
    }

    public async Task<Result<User>> GetCurrentUserAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        return await authService
            .GetUserIdFromAccessToken(request)
            .ThenAsync(id => GetUserByIdAsync(id, cancellationToken))
            .LogIfFailureAsync("Ошибка при получении текущего пользователя");
    }

    public async Task<Result<User>> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await studentRepository.GetByEmailAsync(email, cancellationToken);
    }

    public async Task<Result<User>> RegisterAsync(RegisterRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        var existingUser = await studentRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser.IsSuccess)
            return Error.Conflict("Пользователь с таким email уже существует");


        var hash = passwordManager.HashPassword(request.Password);
        if (hash.IsFailure)
            return hash.Errors.First();

        var newUser = new User(
            Guid.NewGuid(),
            request.Username,
            hash,
            request.Email,
            [(Role)request.Role]
        );

        var savedUser = await studentRepository.AddAsync(newUser, cancellationToken);
        await savedUser.ThenAsync(
            user => authService.GenerateAndSetTokensAsync(
                user,
                (Role)request.Role,
                response,
                cancellationToken
            ));

        return savedUser;
    }

    public async Task<Result> LoginAsync(LoginRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        return await studentRepository
            .GetByEmailAsync(request.Email, cancellationToken)
            .EnsureAsync(user => passwordManager.VerifyPassword(request.Password, user.Password), Error.Unauthorized("Неверный email или пароль"))
            .ThenAsync(user => authService.GenerateAndSetTokensAsync(user, Role.Student, response, cancellationToken));
    }

    public Task<Result> LogoutAsync(HttpResponse response, CancellationToken cancellationToken)
        => authService.ClearTokensAsync(response, cancellationToken);

    public async Task<Result> RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        return await GetCurrentUserAsync(request, cancellationToken)
            .ThenAsync(user => authService.RefreshAccessTokenAsync(request, response, user, cancellationToken));
    }
}
