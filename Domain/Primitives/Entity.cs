using System;

namespace Domain.Primitives;

public abstract class Entity<T> : AuditableEntity, IEquatable<Entity<T>>
{
    protected Entity(T id)
    {
        Id = id;
    }
    protected Entity()
    {
        
    }

    public T Id { get; private init; } = default!;

    public static bool operator ==(Entity<T>? first, Entity<T>? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity<T>? first, Entity<T>? second) =>
        !(first == second);

    
    public bool Equals(Entity<T>? other)
    {
        if (other is null)
        {
            return false;
        }
        if (other.GetType() != GetType())
        {
            return false;
        }
        return EqualityComparer<T>.Default.Equals(other.Id, Id);

    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }
        if (obj is not Entity<T> entity)
        {
            return false;
        }
        return EqualityComparer<T>.Default.Equals(entity.Id, Id);
    }
    public override int GetHashCode() => Id.GetHashCode() * 41;

}