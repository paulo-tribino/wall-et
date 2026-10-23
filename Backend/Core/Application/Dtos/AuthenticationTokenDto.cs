namespace Application.Dtos;

public sealed record AuthenticationTokenDto(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn);
