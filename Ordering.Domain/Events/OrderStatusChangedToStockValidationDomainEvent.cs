namespace Ordering.Domain.Events;

public class OrderStatusChangedToStockValidationDomainEvent : INotification
{
    public int OrderId { get; }

    public OrderStatusChangedToStockValidationDomainEvent(int orderId) => OrderId = orderId;
}

