using Application.Abstractions;
using Domain.Primitives;
using MassTransit;

namespace Infrastructure.MessageBroker;

public class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IDomainEvent
        =>  _publishEndpoint.Publish(message, cancellationToken);

}