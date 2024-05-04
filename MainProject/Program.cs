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

    public static WebApplication Initialize(string[] args)
    {
        Logger.Info("[Program][Main] Start building");
        var builder = WebApplication.CreateBuilder(args);

        var _configuration = new ConfigurationBuilder()
            .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // REGISTER SERVICES HERE
        builder.Services.AddSingleton<IConfiguration>(_configuration);

        PrivateSettings privateSettings = new PrivateSettings();
        _configuration.GetSection("PrivateSettings").Bind(privateSettings);


        Logger.Info("[Program][Main] Building settings");

        builder.Services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));
        builder.Services.Configure<PrivateSettings>(_configuration.GetSection("PrivateSettings"));

        Logger.Info("[Program][Main] Adding services");

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        AppSettings appSettings = new AppSettings();
        appSettings.PrivateSettings = privateSettings;
        _configuration.GetSection("AppSettings").Bind(appSettings);

        Logger.Info("[Program][Main] Initializing swagger doc");

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

        WebApplication app = builder.Build();

        // REGISTER MIDDLEWARE HERE
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

        return app;
    }


    public static void Main(string[] args)
    {
        ReflectionProgram.LaunchConfiguration();
        WebApplication app = Initialize(args);
        app.Run();
        NLog.LogManager.Shutdown(); // Flush and close down internal threads and timers
    }

}