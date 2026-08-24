using Application.Dtos;
using Domain.Entities;

namespace Application.UnitTests.Users;

internal static class UserFixture
{
    public static User CreateUser(
        string name = "John Doe",
        string username = "johndoe",
        string email = "johndoe@example.com",
        string passwordHash = "hashed_password",
        Guid? createdBy = null)
    {
        return User.Create(
            name,
            username,
            email,
            passwordHash,
            createdBy);
    }

    public static UserDto CreateUserDto(
        Guid userId,
        string name = "John Doe",
        string username = "johndoe",
        string email = "johndoe@example.com")
    {
        return new UserDto(
            userId,
            name,
            username,
            email);
    }
}
