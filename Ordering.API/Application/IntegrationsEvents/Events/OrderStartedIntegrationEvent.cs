namespace Ordering.API.Application.IntegrationsEvents.Events;
public record OrderStartedIntegrationEvent : IntegrationEvent
{
    public OrderStartedIntegrationEvent(string userId) => UserId = userId;

    public string UserId { get; init; }
}

