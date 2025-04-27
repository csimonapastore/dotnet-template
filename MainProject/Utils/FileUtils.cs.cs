using System;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using NLog;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Core.Middlewares;
using BasicDotnetTemplate.MainProject.Models.Settings;
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Role;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Utils;

public static class FileUtils
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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

            return JsonSerializer.Deserialize<T>(fileContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Error during file deserialization", ex);
        }
    }

}