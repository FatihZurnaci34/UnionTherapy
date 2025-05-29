namespace UnionTherapy.Domain.Common;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
 
// Guid için varsayılan BaseEntity
public abstract class BaseEntity : BaseEntity<Guid>
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }
} 