using Application.Abstractions.Messaging;

namespace Application.UseCases.Users.Commands.UpdateUserProfile;

public sealed record UpdateUserProfileCommand(
    Guid userId,
    string name,
    string username,
    string email,
    Guid? updatedBy) : ICommand;
