namespace TextAnalytics.Services;

public class FileInputProvider : IInputProvider
{
    private readonly string _filePath;

    public FileInputProvider(string filePath)
    {
        _filePath = filePath;
    }

    public string Read()
    {
        return ReadFromFile(_filePath);
    }

    public string ReadFromFile(string filePath)
    {
        if (!FileExists(filePath))
            throw new FileNotFoundException($"Plik nie istnieje: {filePath}");

        return File.ReadAllText(filePath);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}