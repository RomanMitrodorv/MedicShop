namespace Ordering.UnitTests.Domain;
public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    {
    }

    [Fact]
    public void Create_buyer_item_success()
    {
        var identity = Guid.NewGuid().ToString();
        var name = "fakeUser";

        var fakeBuyerItem = new Buyer(identity, name);

        Assert.NotNull(fakeBuyerItem);
    }


    [Fact]
    public void Create_buyer_item_fail()
    {
        var identity = string.Empty;
        var name = "fakeUser";

        Assert.Throws<ArgumentNullException>(() => new Buyer(identity, name));
    }

    [Fact]
    public void Add_payment_success()
    {
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);
        var orderId = 1;
        var identity = Guid.NewGuid().ToString();
        var name = "fakeUser";

        var fakeBuyerItem = new Buyer(identity, name);

        var result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        Assert.NotNull(result);
    }

    [Fact]
    public void Create_payment_method_success()
    {
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);

        var result = new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, expiration, cardTypeId);

        Assert.NotNull(result);
    }


    [Fact]
    public void create_payment_method_expiration_fail()
    {
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(-1);


        Assert.Throws<OrderingDomainException>(() => new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, expiration, cardTypeId));
    }


    [Fact]
    public void Payment_method_isEqualTo()
    {
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);

        var payment = new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, expiration, cardTypeId);

        var result = payment.IsEqualTo(cardTypeId, cardNumber, expiration);

        Assert.True(result);
    }


    [Fact]
    public void Add_new_PaymentMethod_raises_new_event()
    {
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);
        var orderId = 1;
        var identity = Guid.NewGuid().ToString();
        var name = "fakeUser";
        var expectedResult = 1;

        var fakeBuyerItem = new Buyer(identity, name);

        var result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        Assert.Equal(fakeBuyerItem.DomainEvents.Count, expectedResult);
    }
}

