namespace SharedKernel;

public abstract class Entity
{
    protected Entity(Guid id) => Id = id;

    protected Entity()
    {
    }

    public Guid Id { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public DateTime? DeletedAt { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    public Guid? DeletedBy { get; protected set; }

    public void SetCreated(Guid? userId)
    {
        if (CreatedAt == default)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = userId;
        }
    }

    public void SetUpdated(Guid? userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public void SetDeleted(Guid? userId)
    {
        if (!DeletedAt.HasValue)
        {
            DeletedAt = DateTime.UtcNow;
            DeletedBy = userId;
        }
    }
}
