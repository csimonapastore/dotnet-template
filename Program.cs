using Microsoft.OpenApi.Models;
using BasicDotnetTemplate.Controllers;
using BasicDotnetTemplate.Models.Settings;

namespace BasicDotnetTemplate;
internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // REGISTER SERVICES HERE
        builder.Services.AddSingleton<IConfiguration>(_configuration);

        PrivateSettings privateSettings = new PrivateSettings();
        _configuration.GetSection("PrivateSettings").Bind(privateSettings);

        builder.Services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));
        builder.Services.Configure<PrivateSettings>(_configuration.GetSection("PrivateSettings"));

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        AppSettings appSettings = new AppSettings();
        appSettings.PrivateSettings = privateSettings;
        _configuration.GetSection("AppSettings").Bind(appSettings);

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = appSettings?.Settings?.Version,
                Title = appSettings?.Settings?.Name,
                Description = appSettings?.Settings?.Description,
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });
        });


        var app = builder.Build();

        // REGISTER MIDDLEWARE HERE
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

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

        app.Run();
    }

}