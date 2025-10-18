namespace TextAnalytics.Services;

public interface ILoggerService
{
    void Log(string message);
    void LogError(string message);
}