namespace Lab2.Domain;

public abstract class LibraryItem : IReservable
{
    public int Id { get; }
    public string Title { get;  }
    public bool IsActive { get; protected set; } = true;

    public LibraryItem(int id, string title)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    public abstract void DisplayInfo();
    
    public void Reserve(string email, DateTime from, DateTime to)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Książka nie jest dostępna");
        }
        IsActive = false;
    }

    public void CancelReservation(string email)
    {
        IsActive = true;
    }

    public bool IsAvailable(string email)
    {
        return IsActive;
    }
}