namespace Basket.FunctionalTests;

public class RedisBasketRepositoryTests : BasketScenarioBase
{
    [Fact]
    public async Task Update_Basket_return_and_add_basket()
    {
        using (var server = CreateServer())
        {
            var redis = server.Host.Services.GetRequiredService<ConnectionMultiplexer>();

            var redisBasketRepository = BuildBasketRepository(redis);

            var basket = await redisBasketRepository.UpdateBasketAsync(new CustomerBasket("customerId")
            {
                BuyerId = "buyerId",
                Items = BuildBasketItem()
            });

            Assert.NotNull(basket);

            Assert.Single(basket.Items);
        }
    }

    [Fact]
    public async Task Delete_Basket_return_null()
    {
        using (var server = CreateServer())
        {
            var redis = server.Host.Services.GetRequiredService<ConnectionMultiplexer>();

            var redisBasketRepository = BuildBasketRepository(redis);

            var basket = await redisBasketRepository.UpdateBasketAsync(new CustomerBasket("customerId")
            {
                BuyerId = "buyerId",
                Items = BuildBasketItem()
            });

            var deleteResult = await redisBasketRepository.DeleteBasketAsync(basket.BuyerId);

            var result = await redisBasketRepository.GetBasketAsync(basket.BuyerId);

            Assert.True(deleteResult);
            Assert.Null(result);
        }
    }



    private RedisBasketRepository BuildBasketRepository(ConnectionMultiplexer connMux)
    {
        LoggerFactory loggerFactory = new LoggerFactory();
        return new RedisBasketRepository(loggerFactory, connMux);
    }

    private List<BasketItem> BuildBasketItem()
    {
        return new List<BasketItem>()
        {
           new BasketItem()
           {
                    Id = "basketId",
                    PictureUrl = "pictureurl",
                    ProductId = 1,
                    ProductName = "productName",
                    Quantity = 1,
                    UnitPrice = 1
           }
        };
    }
}

