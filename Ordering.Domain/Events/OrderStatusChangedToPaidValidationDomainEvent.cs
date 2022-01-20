namespace Ordering.Domain.Events;

public class OrderStatusChangedToPaidValidationDomainEvent : INotification
{
    public OrderStatusChangedToPaidValidationDomainEvent(int orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }

    public int OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }
}

