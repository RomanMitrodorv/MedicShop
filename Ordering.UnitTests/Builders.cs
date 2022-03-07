namespace Ordering.UnitTests;

public class AddressBuilder
{
    public Address Build()
    {
        return new Address("street", "city", "country", "zipCode");
    }
}


public class OrderBuilder
{
    public readonly Order order;

    public OrderBuilder(Address address)
    {
        order = new Order("userId", "fakeName", 5, "12", "123", "name", DateTime.UtcNow, address);
    }

    public OrderBuilder AddOne(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        order.AddOrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
        return this;
    }

    public Order Build()
    {
        return order;
    }
}