namespace Ordering.FunctionalTests;

public class OrderingTestsStartup : Startup
{
    public OrderingTestsStartup(IConfiguration env) : base(env)
    {
    }

    public override IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.Configure<RouteOptions>(Configuration);
        return base.ConfigureServices(services);
    }
    protected override void ConfigureAuth(IApplicationBuilder app)
    {
        if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
        {
            app.UseMiddleware<AutoAuthorizeMiddleware>();
        }
        else
        {
            base.ConfigureAuth(app);
        }
    }
}
