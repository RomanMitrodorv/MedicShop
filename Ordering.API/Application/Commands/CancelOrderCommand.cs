namespace Ordering.API.Commands;

public class CancelOrderCommand : IRequest<bool>
{
    public CancelOrderCommand(int orderNumber)
    {
        OrderNumber = orderNumber;
    }

    [DataMember]
    public int OrderNumber { get; set; }
}

