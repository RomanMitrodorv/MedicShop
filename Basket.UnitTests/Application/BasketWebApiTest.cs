namespace Basket.UnitTests.Application;
public class BasketWebApiTest
{
    private readonly Mock<IBasketRepository> _repositoryMock;
    private readonly Mock<IBasketIdentityService> _identityServiceMock;
    private readonly Mock<IEventBus> _serviceBusMock;
    private readonly Mock<ILogger<BasketController>> _loggerMock;

    public BasketWebApiTest()
    {
        _repositoryMock = new Mock<IBasketRepository>();
        _identityServiceMock = new Mock<IBasketIdentityService>();
        _serviceBusMock = new Mock<IEventBus>();
        _loggerMock = new Mock<ILogger<BasketController>>();
    }

    [Fact]
    public async Task Get_customer_basket_success()
    {
        var faceCustomerId = "1";
        var fakeCustomerBasket = GetCustomerBasketFake(faceCustomerId);

        _repositoryMock.Setup(x => x.GetBasketAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(fakeCustomerBasket));

        _identityServiceMock.Setup(x => x.GetUserIdentity()).Returns(faceCustomerId);

        _serviceBusMock.Setup(x => x.Publish(It.IsAny<UserCheckoutAcceptedIntegrationEvent>()));

        var basketController = new BasketController(_loggerMock.Object,
                                                    _repositoryMock.Object,
                                                    _identityServiceMock.Object,
                                                    _serviceBusMock.Object);

        var result = await basketController.GetBasketByIdAsync(faceCustomerId);

        Assert.Equal((result.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);

        Assert.Equal((((ObjectResult)result.Result).Value as CustomerBasket).BuyerId, faceCustomerId);
    }

    [Fact]
    public async Task Post_customer_basket_success()
    {
        var fakeCustomerId = "1";
        var fakeCustomerBasket = GetCustomerBasketFake(fakeCustomerId);

        _repositoryMock.Setup(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>()))
            .Returns(Task.FromResult(fakeCustomerBasket));

        _identityServiceMock.Setup(x => x.GetUserIdentity()).Returns(fakeCustomerId);

        _serviceBusMock.Setup(x => x.Publish(It.IsAny<UserCheckoutAcceptedIntegrationEvent>()));


        var basketController = new BasketController(_loggerMock.Object,
                                                    _repositoryMock.Object,
                                                    _identityServiceMock.Object,
                                                    _serviceBusMock.Object);

        var result = await basketController.UpdateBasketAsync(fakeCustomerBasket);
        Assert.Equal((result.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        Assert.Equal((((ObjectResult)result.Result).Value as CustomerBasket).BuyerId, fakeCustomerId);
    }

    [Fact]
    public async void Doing_Checkout_Without_Basket_Should_Return_Bad_Request()
    {
        var fakeCustomerId = "2";
        _repositoryMock.Setup(x => x.GetBasketAsync(It.IsAny<string>()))
            .Returns(Task.FromResult((CustomerBasket)null));
        _identityServiceMock.Setup(x => x.GetUserIdentity()).Returns(fakeCustomerId);


        var basketController = new BasketController(_loggerMock.Object,
                                                    _repositoryMock.Object,
                                                    _identityServiceMock.Object,
                                                    _serviceBusMock.Object);

        var result = await basketController.CheckoutAsync(new BasketCheckout(), Guid.NewGuid().ToString()) as BadRequestResult;
        Assert.NotNull(result);

    }

    [Fact]
    public async void Doing_Checkout_Wit_Basket_Should_Publish_UserCheckoutAccepted_Integration_Event()
    {
        var fakeCustomerId = "1";
        var fakeCustomerBasket = GetCustomerBasketFake(fakeCustomerId);


        _repositoryMock.Setup(x => x.GetBasketAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(fakeCustomerBasket));

        _identityServiceMock.Setup(x => x.GetUserIdentity()).Returns(fakeCustomerId);


        var basketController = new BasketController(_loggerMock.Object,
                                                    _repositoryMock.Object,
                                                    _identityServiceMock.Object,
                                                    _serviceBusMock.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(
                    new ClaimsIdentity(new Claim[] {
                               new Claim("sub", "testuser"),
                               new Claim("unique_name", "testuser"),
                               new Claim(ClaimTypes.Name, "testuser")
                            }))
                }
            }
        };

        var result = await basketController.CheckoutAsync(new BasketCheckout(), Guid.NewGuid().ToString()) as AcceptedResult;

        _serviceBusMock.Verify(m => m.Publish(It.IsAny<UserCheckoutAcceptedIntegrationEvent>()), Times.Once);

        Assert.NotNull(result);

    }



    private CustomerBasket GetCustomerBasketFake(string faceCustomerId)
    {
        return new CustomerBasket(faceCustomerId)
        {
            Items = new List<BasketItem>()
            {
                new BasketItem()
            }
        };
    }
}

