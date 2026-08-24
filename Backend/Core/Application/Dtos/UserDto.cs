namespace Application.Dtos;

public sealed record UserDto(
    Guid Id,
    string Name,
    string Username,
    string Email);
