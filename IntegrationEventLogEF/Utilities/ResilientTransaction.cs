namespace IntegrationEventLogEF.Utilities
{
    public class ResilientTransaction
    {
        private DbContext _context;

        public ResilientTransaction(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public static ResilientTransaction New(DbContext contxt) => new(contxt);

        public async Task ExecuteAsync(Func<Task> action)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var tran = _context.Database.BeginTransaction())
                {
                    await action();
                    tran.Commit();
                }
            });
        }
    }
}
