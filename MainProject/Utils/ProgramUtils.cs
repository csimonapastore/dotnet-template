using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using NLog;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Core.Middlewares;
using BasicDotnetTemplate.MainProject.Models.Settings;
using BasicDotnetTemplate.MainProject.Services;



namespace BasicDotnetTemplate.MainProject.Utils;

public static class ProgramUtils
{
    private static readonly string[] _newStringArray = Array.Empty<string>();
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

        AppSettings appSettings = new AppSettings();
        appSettings.PrivateSettings = privateSettings;
        _configuration.GetSection("AppSettings").Bind(appSettings);

        builder.Services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));
        builder.Services.Configure<PrivateSettings>(_configuration.GetSection("PrivateSettings"));

        Logger.Info("[ProgramUtils][AddConfiguration] Ended configuration");

        return appSettings;
    }
    public static OpenApiInfo CreateOpenApiInfo(AppSettings appSettings)
    {
        OpenApiInfo openApiInfo = new OpenApiInfo
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

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Inserisci il Bearer Token nel formato **'Bearer {token}'**",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "Inserisci la tua API Key nel campo appropriato.",
                Name = "ApiKey",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    _newStringArray
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    _newStringArray
                }
            });
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
    public static void AddDbContext(ref WebApplicationBuilder builder, AppSettings appSettings)
    {
        Logger.Info("[ProgramUtils][AddDbContext] Adding DbContext");
        var messages = String.Empty;
        if (!String.IsNullOrEmpty(appSettings.DatabaseSettings?.SqlServerConnectionString))
        {
            var connectionString = appSettings.DatabaseSettings?.SqlServerConnectionString ?? String.Empty;
            connectionString = connectionString.Replace("SQLSERVER_DB_SERVER", Environment.GetEnvironmentVariable("SQLSERVER_DB_SERVER"));
            builder.Services.AddDbContext<SqlServerContext>(options =>
                options.UseSqlServer(connectionString)
            );
            messages = "SqlServerContext";
        }

        if (!String.IsNullOrEmpty(appSettings.DatabaseSettings?.MongoDbConnectionString))
        {
            var connectionString = appSettings.DatabaseSettings?.MongoDbConnectionString ?? String.Empty;
            connectionString = connectionString.Replace("MONGO_DB_SERVER", Environment.GetEnvironmentVariable("MONGODB_DB_SERVER"));
            var databaseName = connectionString.Split("/").LastOrDefault();
            if (!String.IsNullOrEmpty(databaseName))
            {
                var client = new MongoClient(connectionString);
                builder.Services.AddDbContext<MongoDbContext>(options =>
                    options.UseMongoDB(client, databaseName)
                );
                messages = messages + (String.IsNullOrEmpty(messages) ? "" : ", ") + "MongoDbContext";
            }
        }

        if (!String.IsNullOrEmpty(appSettings.DatabaseSettings?.PostgreSQLConnectionString))
        {
            var connectionString = appSettings.DatabaseSettings?.PostgreSQLConnectionString ?? String.Empty;
            connectionString = connectionString.Replace("POSTGRESQL_DB_SERVER", Environment.GetEnvironmentVariable("POSTGRESQL_DB_SERVER"));
            builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
            messages = messages + (String.IsNullOrEmpty(messages) ? "" : ", ") + "PostgreSqlDbContext";
        }
        messages = String.IsNullOrEmpty(messages) ? "No context" : messages;
        Logger.Info($"[ProgramUtils][AddDbContext] {messages} added");
    }
    public static void AddScopes(ref WebApplicationBuilder builder)
    {
        Logger.Info("[ProgramUtils][AddScopes] Adding scopes");
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IUserService, UserService>();
        Logger.Info("[ProgramUtils][AddScopes] Done scopes");
    }

    public static void AddAutoMapper(ref WebApplicationBuilder builder)
    {
        Logger.Info("[ProgramUtils][AddAutoMapper] Adding AutoMapperConfiguration");
        builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));
        Logger.Info("[ProgramUtils][AddScopes] Done AutoMapperConfiguration");
    }

}