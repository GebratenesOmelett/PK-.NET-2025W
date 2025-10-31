namespace Lab2.Domain;

public class Book : LibraryItem
{
    public string Author { get; }
    public string ISBN { get; }
    public Book(int id, string title, string author, string isbn) : base(id, title)
    {
        Author = author ?? throw new ArgumentNullException(nameof(author));;
        ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"   Książka: {Title}");
        Console.WriteLine($"   Autor: {Author}");
        Console.WriteLine($"   ISBN: {ISBN}");
        Console.WriteLine($"   Dostępność: {(IsActive ? "Tak" : "Nie")}");
    }
}