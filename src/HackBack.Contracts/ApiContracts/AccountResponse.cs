using ZazaTemplate.Domain;

namespace ZazaTemplate.Contracts.ApiContracts;

public record UserPublic(Guid Id, string Username, string Email, IEnumerable<string> Roles);
