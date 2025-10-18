using System;


namespace TextAnalytics.Services;


public sealed class ConsoleLogger : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"[Info] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }


    public void LogError(string message)
    {
        Console.Error.WriteLine($"[Error] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }
}