namespace Presentation.Endpoints.Users;

internal sealed record UpdateUserProfileRequest(
    string name,
    string username,
    string email);
