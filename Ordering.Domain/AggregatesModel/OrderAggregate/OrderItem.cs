namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderItem : Entity
{
    private string _productName;
    private string _productUrl;
    private int _units;
    private decimal _unitPrice;
    private decimal _discount;

    public int ProductId { get; private set; }

    protected OrderItem() { }

    public OrderItem(string productName, string productUrl, decimal unitPrice, decimal discount, int productId, int units = 1)
    {
        if (units <= 0)
            throw new OrderingDomainException($"Invalid number of units - {units}");

        var total = units * unitPrice;

        if (total < discount)
            throw new OrderingDomainException($"The total of {total} order item is lower than applied discount {discount}");

        _productName = productName;
        _productUrl = productUrl;
        _units = units;
        _unitPrice = unitPrice;
        _discount = discount;
        ProductId = productId;
    }

    public string GetPictureUri() => _productUrl;
    public decimal GetCurrentDiscount() => _discount;
    public string GetOrderItemProductName() => _productName;
    public decimal GetUnitPrice() => _unitPrice;
    public int GetUnits() => _units;


    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
            throw new OrderingDomainException($"Discount is not valid - {discount}");

        _discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units < 0)
            throw new OrderingDomainException($"Units is not valid - {units}");

        _units += units;
    }
}

