namespace Ordering.API.Application.IntegrationsEvents;

public class OrderingIntegrationEventService : IOrderingIntegrationEventService
{
    private readonly ILogger<OrderingIntegrationEventService> _logger;
    private readonly IEventBus _eventBus;
    private readonly OrderingContext _context;
    private readonly IIntegrationEventLogService _eventLogService;
    private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;

    public OrderingIntegrationEventService(ILogger<OrderingIntegrationEventService> logger,
        IEventBus eventBus,
        OrderingContext context,
        IIntegrationEventLogService eventLogService,
        Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        _eventLogService = _integrationEventLogServiceFactory(_context.Database.GetDbConnection());
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

        await _eventLogService.SaveEventAsync(evt, _context.GetCurrentTransaction());
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

