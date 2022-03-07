namespace Ordering.UnitTests.Domain;

public class OrderAggregateTest
{
    public OrderAggregateTest()
    {
    }

    [Fact]
    public void Create_order_item_success()
    {

        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        var fakeOrderItem = new OrderItem(productName, pictureUrl, unitPrice, discount, productId, units);

        Assert.NotNull(fakeOrderItem);
    }

    [Fact]
    public void Invalid_number_of_units()
    {

        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = -1;

        Assert.Throws<OrderingDomainException>(() => new OrderItem(productName, pictureUrl, unitPrice, discount, productId, units));
    }

    [Fact]
    public void Invalid_total_of_order_item_lower_than_discount_applied()
    {

        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 1;

        Assert.Throws<OrderingDomainException>(() => new OrderItem(productName, pictureUrl, unitPrice, discount, productId, units));
    }

    [Fact]
    public void Invalid_discount_setting()
    {
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        var fakeOrderItem = new OrderItem(productName, pictureUrl, unitPrice, discount, productId, units);

        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.SetNewDiscount(-1));
    }

    [Fact]
    public void Invalid_units_setting()
    {
        var productId = 1;
        var productName = "FakeProductName";
        var unitPrice = 12;
        var discount = 15;
        var pictureUrl = "FakeUrl";
        var units = 5;

        var fakeOrderItem = new OrderItem(productName, pictureUrl, unitPrice, discount, productId, units);

        Assert.Throws<OrderingDomainException>(() => fakeOrderItem.AddUnits(-1));
    }

    [Fact]
    public void When_add_two_times_on_the_same_items_then_the_total_of_order_should_be_the_sum_of_the_two_items()
    {
        var addres = new AddressBuilder().Build();

        var order = new OrderBuilder(addres)
            .AddOne(1, "cup", 10.0m, 0, "fakeUrl")
            .AddOne(1, "cup", 10.0m, 0, "fakeUrl")
            .Build();

        Assert.Equal(20.0m, order.GetTotal());
    }

    [Fact]
    public void Add_new_Order_raises_new_event()
    {
        var street = "fakeStreet";
        var city = "FakeCity";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.Now.AddYears(1);
        var expectedResult = 1;

        var fakeAddres = new Address(street, city, country, zipcode);
        var fakeOrder = new Order("1", "fakeName", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, fakeAddres);

        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [Fact]
    public void Add_event_Order_explicitly_raises_new_event()
    {
        var street = "fakeStreet";
        var city = "FakeCity";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.Now.AddYears(1);
        var expectedResult = 2;

        var fakeAddres = new Address(street, city, country, zipcode);
        var fakeOrder = new Order("1", "fakeName", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, fakeAddres);

        fakeOrder.AddDomainEvent(new OrderStartedDomainEvent("userId", "userName", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, fakeOrder));

        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }

    [Fact]
    public void Remove_event_Order_explicitly()
    {
        var street = "fakeStreet";
        var city = "FakeCity";
        var country = "fakeCountry";
        var zipcode = "FakeZipCode";
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.Now.AddYears(1);
        var expectedResult = 1;

        var fakeAddres = new Address(street, city, country, zipcode);
        var fakeOrder = new Order("1", "fakeName", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, fakeAddres);

        var fakeEvent = new OrderStartedDomainEvent("userId", "userName", cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, fakeOrder);

        fakeOrder.AddDomainEvent(fakeEvent);

        fakeOrder.RemoveDomainEvent(fakeEvent);

        Assert.Equal(fakeOrder.DomainEvents.Count, expectedResult);
    }
}

