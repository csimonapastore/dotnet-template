using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Services;

public class BaseService
{
    protected readonly IConfiguration _configuration;
    protected readonly AppSettings _appSettings;
    protected readonly SqlServerContext _sqlServerContext;

    public BaseService(
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    )
    {
        _configuration = configuration;
        _appSettings = new AppSettings();
        _configuration.GetSection("AppSettings").Bind(_appSettings);
        _sqlServerContext = sqlServerContext;
    }
}

