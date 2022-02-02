﻿namespace Basket.API.IntegrationEvents.EventHandling;

public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;
    private readonly IBasketRepository _repository;

    public OrderStartedIntegrationEventHandler(ILogger<OrderStartedIntegrationEventHandler> logger, IBasketRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

            await _repository.DeleteBasketAsync(@event.UserId);
        }
    }
}

