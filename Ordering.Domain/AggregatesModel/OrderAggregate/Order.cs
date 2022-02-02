namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    private DateTime _orderDate;
    private string _description;
    private int _orderStatusId;
    private bool _isDraft;
    private int? _buyerId;
    private int? _paymentMethodId;
    private readonly List<OrderItem> _orderItems;

    public Address Address { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;



    protected Order()
    {
        _isDraft = false;
        _orderItems = new List<OrderItem>();
    }

    public Order(string userId, string userName, int cardTypeId, string cardNumber,
                 string cardSecurityNumber, string cardHolderName, DateTime cardExpiration,
                 Address address, int? buyerId = null, int? paymentMethodId = null) : this()
    {
        _orderDate = DateTime.UtcNow;
        _orderStatusId = OrderStatus.Submitted.Id;
        _buyerId = buyerId;
        _paymentMethodId = paymentMethodId;
        Address = address;

        AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration);
    }


    public static Order IsDraft() => new() { _isDraft = true };
    public int? GetBuyerId => _buyerId;
    public void SetPaymentId(int id) => _paymentMethodId = id;
    public void SetBuyerId(int id) => _buyerId = id;

    public static Order NewDraft()
    {
        var order = new Order();
        order._isDraft = true;
        return order;
    }

    public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        var existingOrderForProduct = _orderItems.SingleOrDefault(x => x.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.GetCurrentDiscount())
                existingOrderForProduct.SetNewDiscount(discount);

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(productName, pictureUrl, unitPrice, discount, productId, units);
            _orderItems.Add(orderItem);
        }
    }
    public void SetAwaitingValidationStatus()
    {
        if (_orderStatusId == OrderStatus.Submitted.Id)
        {
            _orderStatusId = OrderStatus.AwaitingValidation.Id;
            AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, OrderItems));
        }
        else
            StatusChangeException(OrderStatus.AwaitingValidation);
    }

    public void SetStockConfirmedStatus()
    {
        if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
        {
            _orderStatusId = OrderStatus.StockConfirmed.Id;
            _description = "All the items were confirmed with available stock.";
            AddDomainEvent(new OrderStatusChangedToStockValidationDomainEvent(Id));
        }
        else
            StatusChangeException(OrderStatus.StockConfirmed);
    }


    public void SetPaidStatus()
    {
        if (_orderStatusId == OrderStatus.StockConfirmed.Id)
        {
            _orderStatusId = OrderStatus.Paid.Id;
            _description = "The order was paid";
            AddDomainEvent(new OrderStatusChangedToPaidValidationDomainEvent(Id, OrderItems));
        }
        else
            StatusChangeException(OrderStatus.Paid);
    }

    public void SetCancelledStatus()
    {
        if (_orderStatusId == OrderStatus.Shipped.Id ||
           _orderStatusId == OrderStatus.Paid.Id)
        {
            StatusChangeException(OrderStatus.Cancelled);
        }

        _orderStatusId = OrderStatus.Cancelled.Id;
        _description = "The order was cancelled";
        AddDomainEvent(new OrderCancelledDomainEvent(this));

    }


    public void SetShippedStatus()
    {
        if (_orderStatusId != OrderStatus.Paid.Id)
        {
            StatusChangeException(OrderStatus.Shipped);
        }

        _orderStatusId = OrderStatus.Shipped.Id;
        _description = "The order was shipped";
        AddDomainEvent(new OrderShippedDomainEvent(this));

    }

    private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber,
                                          string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
        var domainEvent = new OrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, this);

        AddDomainEvent(domainEvent);
    }

    public decimal GetTotal()
    {
        return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}");
    }
}

