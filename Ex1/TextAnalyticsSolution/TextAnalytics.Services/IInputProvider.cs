namespace TextAnalytics.Services;

public interface IInputProvider
{
    string Read();
    string ReadFromFile(string filePath);
    bool FileExists(string filePath);
}