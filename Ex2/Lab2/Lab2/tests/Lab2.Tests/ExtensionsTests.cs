using Lab2.Domain;
using Lab2.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace Lab2.Tests;

public class ExtensionsTests
{
    [Fact]
    public void Unavailable_FiltersOnlyUnavailableItems()
    {
        var items = new List<LibraryItem>
        {
            new Book(2, "Unavailable Book", "Author", "456") {  }
        };

        var availableItems = items.Available().ToList();

        Assert.Single(availableItems);
        Assert.Equal("Unavailable Book", availableItems[0].Title);
    }

    [Fact]
    public void Newest_ReturnsCorrectNumberOfItems()
    {
        var items = new List<LibraryItem>
        {
            new Book(1, "Old Book", "Author", "123"),
            new Book(2, "New Book", "Author", "456"),
            new Book(3, "Newest Book", "Author", "789")
        };

        var newestItems = items.Newest(2).ToList();

        Assert.Equal(2, newestItems.Count);
        Assert.Equal("Newest Book", newestItems[0].Title);
        Assert.Equal("New Book", newestItems[1].Title);
    }
}