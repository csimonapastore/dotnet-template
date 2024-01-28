using Microsoft.OpenApi.Models;
using BasicDotnetTemplate.Models.Settings;

namespace BasicDotnetTemplate;
internal class Program
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

        builder.Services.Configure<Settings>(_configuration.GetSection("Settings"));

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        Settings settings = new Settings();
        _configuration.GetSection("Settings").Bind(settings);

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = settings.Version,
                Title = settings.Name,
                Description = settings.Description,
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

        // Register IConfiguration in the service container


        var app = builder.Build();

        // REGISTER MIDDLEWARE HERE
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () =>
        {
            return "Hello World!";
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.Run();
    }

}