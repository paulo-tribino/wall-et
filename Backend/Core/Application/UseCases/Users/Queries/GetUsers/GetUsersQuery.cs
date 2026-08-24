using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Dtos.Base;

namespace Application.UseCases.Users.Queries.GetUsers;

public sealed record GetUsersQuery(int page, int pageSize) : IQuery<PagedList<UserDto>>;
