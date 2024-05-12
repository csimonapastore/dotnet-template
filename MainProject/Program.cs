using System;
using System.Runtime.CompilerServices;
using Microsoft.OpenApi.Models;
using NLog;
using BasicDotnetTemplate.MainProject.Models.Settings;
using BasicDotnetTemplate.MainProject.Utils;
using System.Reflection;

namespace BasicDotnetTemplate.MainProject;

public static class ReflectionProgram
{
    public static MethodInfo LaunchConfiguration()
    {
        var a = typeof(Program);

        MethodInfo[] methods = a.GetMethods(); //Using BindingFlags.NonPublic does not show any results
        MethodInfo? initialize = null;

        foreach (MethodInfo m in methods)
        {
            if (m.Name == "Initialize")
            {
                initialize = m;
            }
        }
        return initialize!;
    }
}



internal static class Program
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private static AppSettings AddConfiguration(ref WebApplicationBuilder builder)
    {
        Logger.Info("[Program][AddConfiguration] Adding configuration");

        var _configuration = new ConfigurationBuilder()
            .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        builder.Services.AddSingleton<IConfiguration>(_configuration);

        PrivateSettings privateSettings = new PrivateSettings();
        _configuration.GetSection("PrivateSettings").Bind(privateSettings);

        builder.Services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));
        builder.Services.Configure<PrivateSettings>(_configuration.GetSection("PrivateSettings"));

        AppSettings appSettings = new AppSettings();
        appSettings.PrivateSettings = privateSettings;
        _configuration.GetSection("AppSettings").Bind(appSettings);

        Logger.Info("[Program][AddConfiguration] Ended configuration");

        return appSettings;
    }

    private static void AddOpenApi(ref WebApplicationBuilder builder, AppSettings appSettings)
    {
        Logger.Info("[Program][AddOpenApi] Adding configuration");

        OpenApiInfo openApiInfo = new()
        {
            Version = appSettings.Settings?.Version,
            Title = appSettings.Settings?.Name,
            Description = appSettings.Settings?.Description
        };

        if (!String.IsNullOrEmpty(appSettings.OpenApiSettings?.TermsOfServiceUrl))
        {
            openApiInfo.TermsOfService = new Uri(appSettings.OpenApiSettings.TermsOfServiceUrl);
        }

        if (!String.IsNullOrEmpty(appSettings.OpenApiSettings?.OpenApiContact?.Name) && !String.IsNullOrEmpty(appSettings.OpenApiSettings?.OpenApiContact?.Url))
        {
            openApiInfo.Contact = new OpenApiContact
            {
                Name = appSettings.OpenApiSettings.OpenApiContact.Name,
                Url = new Uri(appSettings.OpenApiSettings.OpenApiContact.Url)
            };
        }

        if (!String.IsNullOrEmpty(appSettings.OpenApiSettings?.OpenApiLicense?.Name) && !String.IsNullOrEmpty(appSettings.OpenApiSettings?.OpenApiLicense?.Url))
        {
            openApiInfo.License = new OpenApiLicense
            {
                Name = appSettings.OpenApiSettings.OpenApiLicense.Name,
                Url = new Uri(appSettings.OpenApiSettings.OpenApiLicense.Url)
            };
        }

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", openApiInfo);
        });

        Logger.Info("[Program][AddOpenApi] Ended configuration");

        return;
    }

    private static void AddServices(ref WebApplicationBuilder builder)
    {
        Logger.Info("[Program][AddServices] Adding services");

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        Logger.Info("[Program][AddServices] Done services");
    }

    private static void AddMiddlewares(ref WebApplication app)
    {
        Logger.Info("[Program][AddMiddlewares] Adding middlewares");

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapControllers(); // This maps all controllers

        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.InjectStylesheet("/swagger-ui/custom.css");
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        Logger.Info("[Program][AddMiddlewares] Done middlewares");
    }

    public static WebApplication Initialize(string[] args)
    {
        Logger.Info("[Program][Initialize] Start building");
        var builder = WebApplication.CreateBuilder(args);

        AppSettings appSettings = Program.AddConfiguration(ref builder);
        Program.AddServices(ref builder);
        Program.AddOpenApi(ref builder, appSettings);
        WebApplication app = builder.Build();
        Program.AddMiddlewares(ref app);
        Logger.Info("[Program][Initialize] End building");
        return app;
    }


    public static void Main(string[] args)
    {
        WebApplication app = Initialize(args);
        Logger.Info("[Program][Main] Launching app");
        app.Run();
        NLog.LogManager.Shutdown(); // Flush and close down internal threads and timers
    }

}