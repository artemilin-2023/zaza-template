using HackBack.Domain;

namespace HackBack.Contracts.ApiContracts;

public record UserPublic(Guid Id, string Username, string Email, IEnumerable<string> Roles);
