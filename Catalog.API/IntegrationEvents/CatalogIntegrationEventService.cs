namespace Catalog.API.IntegrationEvents
{
    public class CatalogIntegrationEventService : ICatalogIntegrationEventService, IDisposable
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly CatalogContext _context;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<IIntegrationEventLogService> _logger;
        private volatile bool disposedValue;

        public CatalogIntegrationEventService(Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
                                              IEventBus eventBus,
                                              CatalogContext context,
                                              ILogger<IIntegrationEventLogService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventLogService = integrationEventLogServiceFactory(_context.Database.GetDbConnection());

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    (_eventLogService as IDisposable)?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task PublishByEventBusAsync(IntegrationEvent @event)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
                await _eventLogService.MarkEventAsInProgressAsync(@event.Id);
                _eventBus.Publish(@event);
                await _eventLogService.MarkEventAsPublishedAsync(@event.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);
                await _eventLogService.MarkEventAsFailedAsync(@event.Id);
            }
        }

        public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent @event)
        {
            _logger.LogInformation("----- CatalogIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", @event.Id);
            await ResilientTransaction.New(_context).ExecuteAsync(async () =>
            {
                await _context.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(@event, _context.Database.CurrentTransaction);
            });

        }
    }
}
