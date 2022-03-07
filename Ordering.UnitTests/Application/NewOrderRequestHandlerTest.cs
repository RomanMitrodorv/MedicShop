namespace Ordering.UnitTests.Application;

public class NewOrderRequestHandlerTest
{
    public readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IOrderingIntegrationEventService> _orderingIntegrationEventService;

    public NewOrderRequestHandlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _identityServiceMock = new Mock<IIdentityService>();
        _mediator = new Mock<IMediator>();
        _orderingIntegrationEventService = new Mock<IOrderingIntegrationEventService>();
    }

    [Fact]
    public async Task Handle_return_false_if_order_is_not_persisted()
    {
        var buyerId = "123";

        var fakeOrderCmd = FakeOrderRequestWithBuyer(new Dictionary<string, object> { ["cardExpiration"] = DateTime.Now.AddYears(1) });

        _orderRepositoryMock.Setup(orderRepo => orderRepo.GetAsync(It.IsAny<int>()))
            .Returns(Task.FromResult<Order>(FakeOrder()));

        _orderRepositoryMock.Setup(orderRepo => orderRepo.UnitOfWork.SaveChangesAsync(default(CancellationToken)))
            .Returns(Task.FromResult(1));

        _identityServiceMock.Setup(svc => svc.GetUserIdentity()).Returns(buyerId);

        var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();

        var handler = new CreateOrderCommandHandler(loggerMock.Object, _mediator.Object, _orderRepositoryMock.Object, _orderingIntegrationEventService.Object);

        var result = await handler.Handle(fakeOrderCmd, new CancellationToken());

        Assert.False(result);
    }

    private Order FakeOrder()
    {
        return new Order("1", "fakeName", 1, "12", "111", "fakeName", DateTime.Now.AddYears(1), new Address("street", "city", "country", "zipcode"));
    }




    private CreateOrderCommand FakeOrderRequestWithBuyer(Dictionary<string, object> args = null)
    {
        return new CreateOrderCommand(
            new List<BasketItem>(),
            userId: args != null && args.ContainsKey("userId") ? (string)args["userId"] : null,
            userName: args != null && args.ContainsKey("userName") ? (string)args["userName"] : null,
            city: args != null && args.ContainsKey("city") ? (string)args["city"] : null,
            street: args != null && args.ContainsKey("street") ? (string)args["street"] : null,
            country: args != null && args.ContainsKey("country") ? (string)args["country"] : null,
            zipcode: args != null && args.ContainsKey("zipcode") ? (string)args["zipcode"] : null,
            cardNumber: args != null && args.ContainsKey("cardNumber") ? (string)args["cardNumber"] : "1234",
            cardExpiration: args != null && args.ContainsKey("cardExpiration") ? (DateTime)args["cardExpiration"] : DateTime.MinValue,
            cardSecurityNumber: args != null && args.ContainsKey("cardSecurityNumber") ? (string)args["cardSecurityNumber"] : "123",
            cardHolderName: args != null && args.ContainsKey("cardHolderName") ? (string)args["cardHolderName"] : "XXX",
            cardTypeId: args != null && args.ContainsKey("cardTypeId") ? (int)args["cardTypeId"] : 0);
    }
}

