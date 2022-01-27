namespace Ordering.Infastructure;
public static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEntities)
        {
            await mediator.Publish(domainEvent);
        }
    }
}

