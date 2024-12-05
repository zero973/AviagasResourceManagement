using ARM.Core.Models.Entities.Intf;

namespace ARM.Core.Models.Entities;

public abstract class BaseActualEntity : IActualEntity
{
    
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool IsActual { get; set; } = true;

    public Guid? CreatedUserId { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public Guid? UpdatedUserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public Guid? DeletedUserId { get; set; }

    public DateTime? DeleteDate { get; set; }

    public override string ToString()
    {
        return $"Id: {Id} IsActual: {IsActual}";
    }

    public bool Equals(IEntity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BaseActualEntity)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
}