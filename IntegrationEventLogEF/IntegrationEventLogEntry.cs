namespace IntegrationEventLogEF
{
    public class IntegrationEventLogEntry
    {
        public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            EventTypeName = @event.GetType().FullName;
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            CreationTime = @event.CreationDate;
            Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true});
            TransactionId = transactionId.ToString();
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonSerializer.Deserialize(Content, type, options: new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) as IntegrationEvent;
            return this;
        }
    }
}
