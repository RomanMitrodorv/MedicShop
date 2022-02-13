namespace Web.HttpAggregator.Services;
public interface IOrderingService
{
    Task<OrderData> GetOrderDraftAsync(BasketData basketData);
}
