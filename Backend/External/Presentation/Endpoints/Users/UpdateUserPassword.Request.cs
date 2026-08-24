namespace Presentation.Endpoints.Users;

internal sealed record UpdateUserPasswordRequest(
    string currentPassword,
    string newPassword);
