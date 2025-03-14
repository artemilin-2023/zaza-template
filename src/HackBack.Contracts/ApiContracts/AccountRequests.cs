using ZazaTemplate.Domain;

namespace ZazaTemplate.Contracts.ApiContracts;

// ������ � ���� ���� �� �.

public enum PublicRole
{
    Student = Role.Student,
    Teacher = Role.Teacher
}

public record RegisterRequest(string Username, string Email, string Password, PublicRole Role);

public record LoginRequest(string Email, string Password);
