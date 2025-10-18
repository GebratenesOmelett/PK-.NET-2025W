using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TextAnalytics.Core;
using TextAnalytics.Services;

namespace TextAnalytics.App;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var serviceProvider = ConfigureServices(args);
            
            var logger = serviceProvider.GetRequiredService<ILoggerService>();
            var inputProvider = serviceProvider.GetRequiredService<IInputProvider>();
            var analyzer = serviceProvider.GetRequiredService<TextAnalyzer>();

            logger.Log("Aplikacja Text Analytics uruchomiona.");

            string text = await GetInputTextAsync(args, inputProvider, logger);
            
            if (string.IsNullOrWhiteSpace(text))
            {
                logger.LogError("Brak tekstu do analizy.");
                return;
            }

            logger.Log("Rozpoczynanie analizy tekstu...");
            var stats = analyzer.Analyze(text);

            DisplayResults(stats);
            await SaveResultsToJsonAsync(stats, "results.json", logger);

            logger.Log("Analiza zakończona pomyślnie.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Krytyczny błąd: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static ServiceProvider ConfigureServices(string[] args)
    {
        var services = new ServiceCollection();

        if (args.Length > 0 && File.Exists(args[0]))
        {
            services.AddTextAnalyticsServices()
                    .AddFileInputProvider(args[0]);
        }
        else
        {
            services.AddTextAnalyticsServices();
        }

        return services.BuildServiceProvider();
    }

    private static async Task<string> GetInputTextAsync(string[] args, IInputProvider inputProvider, ILoggerService logger)
    {
        if (args.Length > 0)
        {
            var filePath = args[0];
            if (inputProvider.FileExists(filePath))
            {
                logger.Log($"Wczytywanie tekstu z pliku: {filePath}");
                return inputProvider.ReadFromFile(filePath);
            }
            else
            {
                logger.LogError($"Plik nie istnieje: {filePath}");
            }
        }

        logger.Log("Wczytywanie tekstu z konsoli...");
        return inputProvider.Read();
    }

    private static void DisplayResults(TextStatistics stats)
    {
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("WYNIKI ANALIZY TEKSTU");
        Console.WriteLine(new string('=', 50));

        Console.WriteLine($"\n📊 PODSTAWOWE STATYSTYKI:");
        Console.WriteLine($"   Znaki (ze spacjami): {stats.CharactersWithSpaces}");
        Console.WriteLine($"   Znaki (bez spacji):  {stats.CharactersWithoutSpaces}");
        Console.WriteLine($"   Litery:              {stats.Letters}");
        Console.WriteLine($"   Cyfry:               {stats.Digits}");
        Console.WriteLine($"   Znaki interpunkcyjne: {stats.Punctuation}");

        Console.WriteLine($"\n📝 ANALIZA SŁÓW:");
        Console.WriteLine($"   Liczba słów:         {stats.WordCount}");
        Console.WriteLine($"   Unikalne słowa:      {stats.UniqueWordCount}");
        Console.WriteLine($"   Najczęstsze słowo:   {stats.MostCommonWord}");
        Console.WriteLine($"   Średnia długość:     {stats.AverageWordLength:F2}");
        Console.WriteLine($"   Najdłuższe słowo:    {stats.LongestWord}");
        Console.WriteLine($"   Najkrótsze słowo:    {stats.ShortestWord}");

        Console.WriteLine($"\n📄 ANALIZA ZDAŃ:");
        Console.WriteLine($"   Liczba zdań:         {stats.SentenceCount}");
        Console.WriteLine($"   Średnia słów/zdanie: {stats.AverageWordsPerSentence:F2}");
        
        var longestSentence = stats.LongestSentence.Length > 100 
            ? stats.LongestSentence.Substring(0, 100) + "..." 
            : stats.LongestSentence;
        Console.WriteLine($"   Najdłuższe zdanie:   {longestSentence}");
    }

    private static async Task SaveResultsToJsonAsync(TextStatistics stats, string fileName, ILoggerService logger)
    {
        try
        {
            var json = JsonConvert.SerializeObject(stats, Formatting.Indented);
            await File.WriteAllTextAsync(fileName, json);
            logger.Log($"Wyniki zapisane do {fileName}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Błąd podczas zapisu do JSON: {ex.Message}");
        }
    }
}