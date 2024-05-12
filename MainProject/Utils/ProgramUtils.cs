using System;
using System.Runtime.CompilerServices;
using Microsoft.OpenApi.Models;
using NLog;
using BasicDotnetTemplate.MainProject.Models.Settings;
using System.Reflection;

namespace BasicDotnetTemplate.MainProject.Utils;

public static class ProgramUtils
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public static AppSettings AddConfiguration(ref WebApplicationBuilder builder, string? path = "", string? filename = "")
    {
        Logger.Info("[ProgramUtils][AddConfiguration] Adding configuration");

        string appSettingsPath = String.IsNullOrEmpty(path) ? System.AppDomain.CurrentDomain.BaseDirectory : path;
        var _configuration = new ConfigurationBuilder()
            .SetBasePath(appSettingsPath)
            .AddJsonFile(String.IsNullOrEmpty(filename) ? "appsettings.json" : filename, optional: false, reloadOnChange: true)
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

        Logger.Info("[ProgramUtils][AddConfiguration] Ended configuration");

        return appSettings;
    }

    public static OpenApiInfo CreateOpenApiInfo(AppSettings appSettings)
    {
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

        return openApiInfo;
    }
    public static void AddOpenApi(ref WebApplicationBuilder builder, AppSettings appSettings)
    {
        Logger.Info("[ProgramUtils][AddOpenApi] Adding swagger doc");

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", CreateOpenApiInfo(appSettings));
        });

        Logger.Info("[ProgramUtils][AddOpenApi] Ended swagger doc");
    }

    public static void AddServices(ref WebApplicationBuilder builder)
    {
        Logger.Info("[ProgramUtils][AddServices] Adding services");

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        Logger.Info("[ProgramUtils][AddServices] Done services");
    }

    public static void AddMiddlewares(ref WebApplication app)
    {
        Logger.Info("[ProgramUtils][AddMiddlewares] Adding middlewares");

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

        Logger.Info("[ProgramUtils][AddMiddlewares] Done middlewares");
    }

}