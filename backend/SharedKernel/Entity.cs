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

    public void OnEntityCreated()
    {
        if (CreatedAt == default)
        {
            CreatedAt = DateTime.UtcNow;
        }
    }

    public void OnEntityUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void OnEntityDeleted()
    {
        if (!DeletedAt.HasValue)
        {
            DeletedAt = DateTime.UtcNow;
        }
    }

    public void SetCreatedBy(Guid? userId)
    {
        if (!CreatedBy.HasValue)
        {
            CreatedBy = userId;
        }
    }

    public void SetUpdatedBy(Guid? userId)
    {
        UpdatedBy = userId;
    }

    public void SetDeletedBy(Guid? userId)
    {
        if (!DeletedBy.HasValue)
        {
            DeletedBy = userId;
        }
    }
}
