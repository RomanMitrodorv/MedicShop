namespace Ordering.API.Queries;

public class OrderQueries : IOrderQueries
{
    private string _connectionString = string.Empty;

    public OrderQueries(string conntectionString)
    {
        _connectionString = !string.IsNullOrWhiteSpace(conntectionString) ? conntectionString : throw new ArgumentNullException(conntectionString);
    }

    public Task<IEnumerable<CardType>> GetCardTypesAsync()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            return connection.QueryAsync<CardType>("select * from ordering.cardtypes");
        }
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var result = await connection.QueryAsync<dynamic>(
                @"select o.[Id] as ordernumber,o.OrderDate as date, o.Description as description,
                    o.Address_City as city, o.Address_Country as country, o.Address_Street as street, o.Address_ZipCode as zipcode,
                    os.Name as status, 
                    oi.ProductName as productname, oi.Units as units, oi.UnitPrice as unitprice, oi.PictureUrl as pictureurl
                    FROM ordering.Orders o
                    LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid 
                    LEFT JOIN ordering.orderstatus os on o.OrderStatusId = os.Id
                    WHERE o.Id=@id"
                    , new { id }
                );

            if (result.AsList().Count == 0)
                throw new KeyNotFoundException();

            return MapOrderItems(result);
        }
    }

    public Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {

            connection.Open();

            return connection.QueryAsync<OrderSummary>(@"select o.[Id] as ordernumber, o.[OrderDate] as [date], s.[Name] as [status], sum(i.units * i.unitprice) as total
                from [ordering].[orders]            as o
                left join [ordering].[orderitems]  as i on i.orderid = o.id
                left join [ordering].[orderstatus] as s on s.Id = o.OrderStatusId
                left join [ordering].[buyers]      as b on b.Id = o.BuyerId
                where b.IdentityGuid = @userId
                group by o.[Id], o.[OrderDate], s.[Name] 
                order by o.[Id]", new { userId });
        }
    }



    private Order MapOrderItems(dynamic result)
    {
        var order = new Order()
        {
            ordernumber = result[0].ordernumber,
            date = result[0].date,
            status = result[0].status,
            description = result[0].description,
            street = result[0].street,
            city = result[0].city,
            zipcode = result[0].zipcode,
            country = result[0].country,
            orderitems = new List<Orderitem>(),
            total = 0
        };

        foreach (dynamic item in result)
        {
            var orderitem = new Orderitem
            {
                productname = item.productname,
                units = item.units,
                unitprice = (double)item.unitprice,
                pictureurl = item.pictureurl
            };

            order.total += item.units * item.unitprice;
            order.orderitems.Add(orderitem);
        }

        return order;
    }
}
