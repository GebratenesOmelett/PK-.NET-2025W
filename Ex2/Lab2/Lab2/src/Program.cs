using System;
using System.Linq;
using Lab2.Domain;
using Lab2.Extensions;
using Lab2.Services;

class Program
{
    static void Main(string[] args)
    {
        var library = new LibraryService();
        var analytics = new AnalyticsService(library);
        
        library.OnNewReservation += r => Console.WriteLine($"[INFO] Nowa rezerwacja: {r.Item.Title} dla {r.Email} ({r.From:d} - {r.To:d})");
        
        library.OnReservationCancelled += r => Console.WriteLine($"[INFO] Anulowano rezerwację: {r.Item.Title} dla {r.Email}");

        while (true)
        {
            try
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddBook(library); break;
                    case "2": AddEBook(library); break;
                    case "3": RegisterUser(library); break;
                    case "4": ShowAvailableItems(library); break;
                    case "5": SearchItems(library); break;
                    case "6": CreateReservation(library); break;
                    case "7": CancelReservation(library); break;
                    case "8": ShowUserReservations(library); break;
                    case "9": ShowStatistics(analytics); break;
                    case "0": return;
                    default: Console.WriteLine("Nieznana opcja."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Błąd: {ex.Message}");
            }
        }
    }
    static void DisplayMenu()
    {
        Console.WriteLine("\n=== Menu Główne ===");
        Console.WriteLine("1. Dodaj książkę");
        Console.WriteLine("2. Dodaj e-booka");
        Console.WriteLine("3. Zarejestruj użytkownika");
        Console.WriteLine("4. Pokaż dostępne pozycje");
        Console.WriteLine("5. Wyszukaj pozycje");
        Console.WriteLine("6. Zarezerwuj pozycję");
        Console.WriteLine("7. Anuluj rezerwację");
        Console.WriteLine("8. Moje rezerwacje");
        Console.WriteLine("9. Statystyki");
        Console.WriteLine("0. Wyjście");
        Console.Write("Wybierz opcję: ");
    }

    static void AddBook(LibraryService library)
    {
        Console.Write("Tytuł: "); var title = Console.ReadLine();
        Console.Write("Autor: "); var author = Console.ReadLine();
        Console.Write("ISBN: "); var isbn = Console.ReadLine();
        
        library.AddItem(new Book(library.NextId(), title, author, isbn));
        Console.WriteLine("Dodano książkę.");
    }

    static void AddEBook(LibraryService library)
    {
        Console.Write("Tytuł: "); var title = Console.ReadLine();
        Console.Write("Autor: "); var author = Console.ReadLine();
        Console.Write("ISBN: "); var isbn = Console.ReadLine();
        Console.Write("Format: "); var format = Console.ReadLine();
        
        library.AddItem(new EBook(library.NextId(), title, author, isbn, format));
        Console.WriteLine("Dodano e-booka.");
    }

    static void RegisterUser(LibraryService library)
    {
        Console.Write("Email użytkownika: "); var email = Console.ReadLine();
        library.RegisterUser(email);
        Console.WriteLine("Zarejestrowano użytkownika.");
    }

    static void ShowAvailableItems(LibraryService library)
    {
        var availableItems = library.ListAvailableItems();
        
        if (!availableItems.Any())
        {
            Console.WriteLine("Brak dostępnych pozycji.");
            return;
        }

        Console.WriteLine("\nDostępne pozycje:");
        foreach (var item in availableItems)
            item.DisplayInfo();
    }

    static void SearchItems(LibraryService library)
    {
        Console.Write("Szukaj (tytuł/autor): "); var searchTerm = Console.ReadLine();
        var results = library.SearchItems(searchTerm);
        
        if (!results.Any())
        {
            Console.WriteLine("Nie znaleziono pozycji.");
            return;
        }

        Console.WriteLine($"\nWyniki wyszukiwania dla '{searchTerm}':");
        foreach (var item in results)
            item.DisplayInfo();
    }

    static void CreateReservation(LibraryService library)
    {
        Console.Write("ID pozycji: "); 
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Nieprawidłowy ID.");
            return;
        }

        Console.Write("Email: "); var email = Console.ReadLine();
        var from = DateTime.Now;
        var to = from.AddDays(14); 

        var reservation = library.CreateReservation(id, email, from, to);
        Console.WriteLine($"Zarezerwowano: {reservation.Item.Title} do {reservation.To:d}");
    }

    static void CancelReservation(LibraryService library)
    {
        Console.Write("ID rezerwacji: "); 
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Nieprawidłowy ID.");
            return;
        }

        library.CancelReservation(id);
        Console.WriteLine("Anulowano rezerwację.");
    }

    static void ShowUserReservations(LibraryService library)
    {
        Console.Write("Email użytkownika: "); var email = Console.ReadLine();
        var reservations = library.GetUserReservations(email);
        
        if (!reservations.Any())
        {
            Console.WriteLine("Brak aktywnych rezerwacji.");
            return;
        }

        Console.WriteLine($"\nRezerwacje dla {email}:");
        foreach (var reservation in reservations)
        {
            Console.WriteLine($"   ID: {reservation.Id}, {reservation.Item.Title}");
            Console.WriteLine($"   Okres: {reservation.From:d} - {reservation.To:d}");
        }
    }

    static void ShowStatistics(AnalyticsService analytics)
    {
        Console.WriteLine("\n  Statystyki:");
        Console.WriteLine($"   Średni czas wypożyczenia: {analytics.AverageLoanLengthDays():F2} dni");
        Console.WriteLine($"   Najpopularniejszy tytuł: {analytics.MostPopularItemTitle()}");
        Console.WriteLine($"   Łączna liczba wypożyczeń: {analytics.TotalLoans()}");
        Console.WriteLine($"   Wskaźnik realizacji: {analytics.FulfillmentRate():F1}%");
        
        Console.Write("   Oblicz wynik popularności dla tytułu: ");
        var title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title))
        {
            var score = analytics.LogPopularityScore(title);
            Console.WriteLine($"   Wynik popularności '{title}': {score:F2}");
        }
    }
}