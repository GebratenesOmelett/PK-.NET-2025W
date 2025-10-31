namespace Lab2.Domain;

public class Reservation
{
    public int Id { get; }
    public LibraryItem Item { get; }
    public string Email { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public bool IsActive { get; private set; } = true;

    public Reservation(int id, LibraryItem item, string email, DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentException("Wypożyczenie musi być wcześniej niż czas oddania");
        }
        
        Id = id;
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        From = from;
        To = to;
    }

    public void Cancel()
    {
        IsActive = false;
        Item.CancelReservation(Email);
    }
    
    public bool ConflictsWith(DateTime from, DateTime to)
    {
        return IsActive && From < to && To > from;
    }
}