using SharedKernel;

namespace Domain.Errors;

public sealed class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "A user with the specified ID was not found.");

    public static readonly Error EmailAlreadyInUse = new("User.EmailAlreadyInUse", "The provided email address is already in use.");

    public static readonly Error UsernameAlreadyInUse = new("User.UsernameAlreadyInUse", "The provided username is already in use.");

    public static readonly Error InvalidCurrentPassword = new("User.InvalidCurrentPassword", "The current password provided is incorrect.");
}
