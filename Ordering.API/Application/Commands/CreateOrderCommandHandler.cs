namespace Ordering.API.Application.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{

    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IOrderRepository _repository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;

    public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IMediator mediator, IOrderRepository repository, IOrderingIntegrationEventService orderingIntegrationEventService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderStartedEvent = new OrderStartedIntegrationEvent(request.UserId);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedEvent);

        var address = new Address(request.Street, request.City, request.Country, request.ZipCode);
        var order = new Domain.AggregatesModel.OrderAggregate.Order(request.UserId, request.UserName, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpiration, address);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
        }

        _logger.LogInformation("----- Creating Order - Order: {@Order}", order);
        _repository.Add(order);

        return await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool>
{
    public CreateOrderIdentifiedCommandHandler(
        IMediator mediator,
        IRequestManager requestManager,
        ILogger<IdentifiedCommandHandler<CreateOrderCommand, bool>> logger)
        : base(mediator, requestManager, logger)
    {
    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true;                // Ignore duplicate requests for creating order.
    }
}
