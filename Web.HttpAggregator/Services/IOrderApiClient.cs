namespace Web.HttpAggregator.Services;

public interface IOrderApiClient
{
    Task<OrderData> GetOrderDraftFromBasketAsync(BasketData basketData);
}

