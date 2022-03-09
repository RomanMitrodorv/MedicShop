namespace Ordering.API.Application.IntegrationsEvents;

public class OrderingIntegrationEventService : IOrderingIntegrationEventService
{
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
    private readonly IEventBus _eventBus;
    private readonly OrderingContext _orderingContext;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly ILogger<OrderingIntegrationEventService> _logger;

    public OrderingIntegrationEventService(IEventBus eventBus,
        OrderingContext orderingContext,
        IntegrationEventLogContext eventLogContext,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
        ILogger<OrderingIntegrationEventService> logger)
    {
        _orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
        _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _eventLogService = _integrationEventLogServiceFactory(_orderingContext.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

        await _eventLogService.SaveEventAsync(evt, _orderingContext.GetCurrentTransaction());
    }

    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var events = await _eventLogService.GetEventLogsPendingToPublishAsync(transactionId);

        foreach (var e in events)
        {
            _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", e.EventId, Program.AppName, e.IntegrationEvent);

            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(e.EventId);
                _eventBus.Publish(e.IntegrationEvent);
                await _eventLogService.MarkEventAsPublishedAsync(e.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", e.EventId, Program.AppName);

                await _eventLogService.MarkEventAsFailedAsync(e.EventId);

            }
        }
    }
}

