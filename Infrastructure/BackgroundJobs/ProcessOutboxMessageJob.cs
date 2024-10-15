using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence.Data;
using Persistence.Outbox;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public class ProcessOutboxMessageJob : IJob
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IPublisher _publisher;


    public ProcessOutboxMessageJob(ApplicationDbContext applicationDbContext, IPublisher publisher)
    {
        _applicationDbContext = applicationDbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _applicationDbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);
        
        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            await _publisher.Publish(domainEvent, context.CancellationToken);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _applicationDbContext.SaveChangesAsync();
    }
}