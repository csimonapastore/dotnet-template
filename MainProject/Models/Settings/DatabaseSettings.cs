namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class DatabaseSettings
{
#nullable enable
    public string? SqlServerConnectionString { get; set; }
    public string? MongoDbConnectionString { get; set; }
    public string? PostgreSQLConnectionString { get; set; }
#nullable disable
}