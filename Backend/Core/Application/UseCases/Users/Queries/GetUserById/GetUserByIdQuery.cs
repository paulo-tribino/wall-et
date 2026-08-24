using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.UseCases.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid userId) : IQuery<UserDto>;
