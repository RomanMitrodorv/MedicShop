namespace Ordering.API.Infastructure.Services;

public class IdentityService : IIdentityService
{
    private IHttpContextAccessor _contextAccessor;

    public IdentityService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public string GetUserIdentity()
    {
        return _contextAccessor.HttpContext.User.FindFirst("sub").Value;
    }

    public string GetUserName()
    {
        return _contextAccessor.HttpContext.User.Identity.Name;
    }
}


