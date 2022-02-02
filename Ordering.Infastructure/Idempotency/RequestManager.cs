namespace Ordering.Infastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly OrderingContext _context;

    public RequestManager(OrderingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exist = await ExistAsync(id);
        var request = exist ? throw new OrderingDomainException($"Request with {id} already exists") : new ClientRequest()
        {
            Id = id,
            Name = typeof(T).Name,
            Time = DateTime.UtcNow
        };

        _context.Add(request);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _context.FindAsync<ClientRequest>(id);
        return request != null;
    }
}

