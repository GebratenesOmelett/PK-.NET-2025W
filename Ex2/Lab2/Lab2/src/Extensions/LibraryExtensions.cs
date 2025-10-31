using Lab2.Domain;

namespace Lab2.Extensions;

public static class LibraryExtensions
{
    public static IEnumerable<T> Available<T>(this IEnumerable<T> items) where T : LibraryItem
    {
        return items.Where(i => i.IsActive);
    }

    public static IEnumerable<LibraryItem> Newest(this IEnumerable<LibraryItem> items, int take)
    {
        return items.OrderByDescending(i => i.Id).Take(take);
    }

    public static IEnumerable<LibraryItem> ByAuthor(this IEnumerable<LibraryItem> items, string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return items;
        }

        return items.Where(item => item is Book book && book.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
    }
    public static IEnumerable<LibraryItem> WithTitleContaining(this IEnumerable<LibraryItem> items, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return items;
        }

        return items.Where(item => item.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
