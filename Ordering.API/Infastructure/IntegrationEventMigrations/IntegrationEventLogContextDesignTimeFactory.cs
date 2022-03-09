namespace Catalog.API.IntegrationEventMigrations
{
    public class IntegrationEventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<IntegrationEventLogContext>
    {
        public IntegrationEventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IntegrationEventLogContext>();

            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=OrderingDb;Integrated Security=true");

            return new IntegrationEventLogContext(optionsBuilder.Options);
        }
    }
}
