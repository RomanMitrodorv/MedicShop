namespace Catalog.API.IntegrationEvents.Events
{
    public record ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public ProductPriceChangedIntegrationEvent(decimal newPrice, decimal oldPrice, int productId)
        {
            NewPrice = newPrice;
            OldPrice = oldPrice;
            ProductId = productId;
        }

        public decimal NewPrice { get; private init; }
        public decimal OldPrice { get; private init; }
        public int ProductId { get; private init; }

    }
}
