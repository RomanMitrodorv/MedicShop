﻿namespace Ordering.Domain.AggregatesModel.BuyerAggregate;

public class Buyer : Entity, IAggregateRoot
{
    public string IdentityGuid { get; private set; }
    public string Name;
    private List<PaymentMethod> _paymentMethods;
    public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    protected Buyer()
    {
        _paymentMethods = new List<PaymentMethod>();
    }

    public Buyer(string identity, string name) : this()
    {
        IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new OrderingDomainException(nameof(identity));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new OrderingDomainException(nameof(name));
    }

    public PaymentMethod VerifyOrAddPaymentMethod(
        int cardTypeId, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateTime expiration, int orderId)
    {
        var existingPayment = _paymentMethods.SingleOrDefault(x => x.IsEqualTo(cardTypeId, cardNumber, expiration));

        if (existingPayment != null)
        {
            AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));
            return existingPayment;
        }

        var payment = new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, expiration, cardTypeId);

        _paymentMethods.Add(payment);
        AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));
        return payment;
    }
}
