using ARM.Core.Models.Entities.Intf;

namespace ARM.DAL.Models.Entities;

public abstract class BaseEntity : IEntity
{
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public override string ToString()
    {
        return $"Id: {Id}";
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