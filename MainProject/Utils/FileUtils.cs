using System.Text.Json;
using NLog;



namespace BasicDotnetTemplate.MainProject.Utils;

public static class FileUtils
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? ConvertFileToObject<T>(string? filePath = "")
    {
        Logger.Info("[FileUtils][ReadJson] Reading file");

        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("filePath cannot be null or empty", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file does not exists", filePath);
        }

        try
        {
            string fileContent = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<T>(fileContent, jsonSerializerOptions);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Error during file deserialization", ex);
        }
    }

}
