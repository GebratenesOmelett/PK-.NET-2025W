namespace TextAnalytics.Services;

public class ConsoleInputProvider : IInputProvider
{
    public string Read()
    {
        Console.WriteLine("Wprowadź tekst do analizy (wpisz 'END' w nowej linii aby zakończyć):");
        
        var lines = new List<string>();
        string line;
        
        while ((line = Console.ReadLine()) != null && line != "END")
        {
            lines.Add(line);
        }
        
        return string.Join(Environment.NewLine, lines);
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