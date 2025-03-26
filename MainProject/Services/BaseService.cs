using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Services;

public class BaseService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IConfiguration _configuration;
    protected readonly AppSettings _appSettings;
    protected readonly SqlServerContext _sqlServerContext;

    public BaseService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _appSettings = new AppSettings();
        _configuration.GetSection("AppSettings").Bind(_appSettings);
        _sqlServerContext = sqlServerContext;
    }

    protected int? GetCurrentUserId()
    {
        int? userId = null;
        var user = this.GetCurrentUser();
        if (user != null)
        {
            userId = this._sqlServerContext.Users.Where(x => !x.IsDeleted && x.Guid == user.Guid).FirstOrDefault()?.Id;
        }
        return userId;
    }

    protected AuthenticatedUser? GetCurrentUser()
    {
        return _httpContextAccessor.HttpContext?.Items["User"] as AuthenticatedUser;
    }
}

