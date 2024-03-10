namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class DatabaseConnection
{
#nullable enable
    public string? SqlServer { get; set; }
    public string? Postgres { get; set; }
    public string? Mongodb { get; set; }
#nullable disable
}