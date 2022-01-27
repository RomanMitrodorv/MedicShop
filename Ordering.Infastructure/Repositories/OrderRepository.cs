namespace Ordering.Infastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _orderingContext;

    public OrderRepository(OrderingContext orderingContext)
    {
        _orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
    }

    public IUnitOfWork UnitOfWork { get { return _orderingContext; } }

    public Order Add(Order order)
    {
        return _orderingContext.Add(order).Entity;
    }

    public async Task<Order> GetAsync(int orderId)
    {
        var order = await _orderingContext.Orders
                                          .Include(x => x.Address)
                                          .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
        {
            order = _orderingContext
                        .Orders
                        .Local
                        .FirstOrDefault(o => o.Id == orderId);
        }
        if (order != null)
        {
            await _orderingContext.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();
            await _orderingContext.Entry(order)
                .Reference(i => i.OrderStatus).LoadAsync();
        }
        return order;
    }

    public void Update(Order order)
    {
        _orderingContext.Entry(order).State = EntityState.Modified;
    }
}

