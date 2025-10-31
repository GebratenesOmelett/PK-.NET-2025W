namespace Lab2.Services;

public class AnalyticsService
{
    private LibraryService _libraryService;
    
    public AnalyticsService(LibraryService libraryService)
    {
        _libraryService = libraryService ?? throw new ArgumentNullException(nameof(libraryService));
    }

    public double AverageLoanLengthDays()
    {
        var completeReservations = _libraryService.GetAllReservations().Where(r => !r.IsActive || r.To < DateTime.Now);

        if (!completeReservations.Any())
        {
            return 0;
        } 
        return completeReservations.Average(r => (r.To - r.From).TotalDays);
    }

    public int TotalLoans()
    {
        return _libraryService.GetAllReservations().Count(r => !r.IsActive || r.To < DateTime.Now);
    }

    public string MostPopularItemTitle()
    {
        var itemReservationCount = _libraryService.GetAllReservations()
            .GroupBy(r => r.Item.Title)
            .Select(g => new { Title = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        if (!itemReservationCount.Any())
        {
            return "Nie ma jeszcze rezerwacji";
        }
        
        var maxCount = itemReservationCount.First().Count;
        var mostPopular = itemReservationCount.Where(x => x.Count == maxCount).ToList();

        return mostPopular.Count == 1 
            ? mostPopular.First().Title 
            : $"Kilka rzeczy z {maxCount} rezerwacjami";
    }
    
    public double FulfillmentRate()
    {
        var allReservations = _libraryService.GetAllReservations().ToList();
        if (!allReservations.Any())
        {
            return 0;
        }

        var fulfilledReservations = allReservations.Count(r => !r.IsActive || r.To < DateTime.Now);
        return (double)fulfilledReservations / allReservations.Count * 100;
    }
    public double LogPopularityScore(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Tytuł nie może być pusty");
        }

        var reservationCount = _libraryService.GetAllReservations().Count(r => r.Item.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (reservationCount <= 0)
        {
            return 0;
        }

        return Math.Log(reservationCount + 1); 
    }
    
}