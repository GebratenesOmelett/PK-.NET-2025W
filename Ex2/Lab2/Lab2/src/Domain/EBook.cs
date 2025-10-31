namespace Lab2.Domain;

public class EBook : Book
{
    public string Format { get; }

    public EBook(int id, string title, string author, string isbn, string format) : base(id, title, author, isbn)
    {
        Format = format ?? throw new ArgumentNullException(nameof(format));
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"   EBook: {Title}");
        Console.WriteLine($"   Autor: {Author}");
        Console.WriteLine($"   ISBN: {ISBN}");
        Console.WriteLine($"   Format: {Format}");
        Console.WriteLine($"   Dostępność: {(IsActive ? "Yes" : "No")}");
    }
}