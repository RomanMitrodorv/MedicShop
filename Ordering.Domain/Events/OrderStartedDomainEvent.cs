namespace Ordering.Domain.Events;

public class OrderStartedDomainEvent : INotification
{
    public OrderStartedDomainEvent(string userId,
                                   string userName,
                                   int cardTypeId,
                                   string cardNumber,
                                   string cardSecurityNumber,
                                   string carHolderName,
                                   DateTime cardExpiration,
                                   Order order)
    {
        UserId = userId;
        UserName = userName;
        CardTypeId = cardTypeId;
        CardNumber = cardNumber;
        CardSecurityNumber = cardSecurityNumber;
        CarHolderName = carHolderName;
        CardExpiration = cardExpiration;
        Order = order;
    }

    public string UserId { get; }
    public string UserName { get; }
    public int CardTypeId { get; }
    public string CardNumber { get; }
    public string CardSecurityNumber { get; }
    public string CarHolderName { get; }
    public DateTime CardExpiration { get; }
    public Order Order { get; }

}

