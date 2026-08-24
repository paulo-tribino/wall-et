namespace Presentation.Endpoints.Users;

internal sealed record CreateUserRequest(
   string name,
   string username,
   string email,
   string password);
