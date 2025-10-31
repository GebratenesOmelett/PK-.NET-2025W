using Lab2.Domain;
using Lab2.Services;
using Xunit;
using Assert = Xunit.Assert;


namespace Lab2.Tests;

public class AnalyticsServiceTests
{
    [Fact]
    public void AverageLoanLengthDays_NoReservations_ReturnsZero()
    {
        var libraryService = new LibraryService();
        var analytics = new AnalyticsService(libraryService);
        
        var result = analytics.AverageLoanLengthDays();
        
        Assert.Equal(0, result);
    }

    [Fact]
    public void MostPopularItemTitle_NoReservations_ReturnsDefaultMessage()
    {
        var libraryService = new LibraryService();
        var analytics = new AnalyticsService(libraryService);
        
        var result = analytics.MostPopularItemTitle();
        
        Assert.Equal("Nie ma jeszcze rezerwacji", result);
    }

    [Fact]
    public void LogPopularityScore_EmptyTitle_ThrowsException()
    {
        var libraryService = new LibraryService();
        var analytics = new AnalyticsService(libraryService);
        
        Assert.Throws<ArgumentException>(() => analytics.LogPopularityScore(""));
    }

    [Fact]
    public void LogPopularityScore_ValidTitle_ReturnsPositiveValue()
    {
        var libraryService = new LibraryService();
        var analytics = new AnalyticsService(libraryService);
        var book = new Book(1, "Popular Book", "Author", "1234567890");
        libraryService.AddItem(book);
        libraryService.RegisterUser("test@email.com");
        
        libraryService.CreateReservation(1, "test@email.com", 
            DateTime.Now, DateTime.Now.AddDays(7));
        var score = analytics.LogPopularityScore("Popular Book");
        
        Assert.True(score > 0);
    }
}
