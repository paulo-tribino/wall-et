using SharedKernel;

namespace Domain.Entities;

public sealed class User : Entity
{
    private User(
        Guid id,
        string name,
        string username,
        string email,
        string passwordHash)
        : base(id)
    {
        Name = name;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }

    private User()
    {
    }

    public string Name { get; private set; }

    public string Username { get; private set; }

    public string Email { get; private set; }

    public string? PasswordHash { get; private set; }

    public static User Create(
        string name,
        string username,
        string email,
        string passwordHash,
        Guid? userId)
    {
        var user = new User(
            Guid.CreateVersion7(),
            name,
            username,
            email,
            passwordHash);

        user.SetCreated(userId);

        return user;
    }

    public void UpdateProfile(
        string name,
        string username,
        string email,
        Guid? userId)
    {
        Name = name;
        Username = username;
        Email = email;

        SetUpdated(userId);
    }

    public void UpdatePassword(
        string passwordHash,
        Guid? userId)
    {
        PasswordHash = passwordHash;

        SetUpdated(userId);
    }

    public void Delete(Guid? userId)
    {
        SetDeleted(userId);
    }
}
