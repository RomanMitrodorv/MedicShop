namespace Catalog.API.IntegrationEvents
{
    public interface ICatalogIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent @event);
        Task PublishByEventBusAsync(IntegrationEvent @event);
    }
}
