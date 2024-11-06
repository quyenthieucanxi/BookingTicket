namespace Domain.Primitives;

public abstract class AggregateRoot<TValue> : Entity<TValue>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(TValue id)
        : base(id)
    {
        
    }
    protected AggregateRoot()
    {
    }
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}