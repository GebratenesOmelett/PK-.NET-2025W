using Lab2.Domain;
using Lab2.Exceptions;

namespace Lab2.Services;

public class LibraryService
{
    private List<LibraryItem> _items = new();
    private List<Reservation> _reservations = new();
    private readonly List<string> _users = new();
    private int _nextItemId = 1;
    private int _nextReservationId = 1;
    
    public event Action<Reservation> OnNewReservation;
    public event Action<Reservation> OnReservationCancelled;

    public int NextId()
    {
        return _nextItemId++;
    }

    public void AddItem(LibraryItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        _items.Add(item);
    }

    public void RegisterUser(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email nie może być pusty");
        }

        if (_users.Contains(email))
        {
            throw new InvalidOperationException("Użytkownik już istnieje");
        }
        _users.Add(email);
    }

    public IEnumerable<LibraryItem> ListAvailableItems()
    {
        return _items.Where(item => item.IsActive);
    }

    public IEnumerable<LibraryItem> SearchItems(string search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return _items;
        
        return _items.Where(item => item.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || (item is Book book && book.Author.Contains(search, StringComparison.OrdinalIgnoreCase)));
    }

    public Reservation CreateReservation(int itemId, string userEmail, DateTime from, DateTime to)
    {
        if (from >= to)
        {
            throw new ArgumentException("Końcowa data musi być przed początkową");
        }

        if (!_users.Contains(userEmail))
        {
            throw new ArgumentException("Użytkownik jest niezarejestrowany");
        }

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new ArgumentException("Nie odnaleziono");
        }

        if (!item.IsActive)
        {
            throw new ReservationConflictException("Nie jest dostępny");
        }

        var conflictingReservation = _reservations.FirstOrDefault(r => r.Item.Id == itemId && r.ConflictsWith(from, to));

        if (conflictingReservation != null)
        {
            throw new ReservationConflictException($"Rzecz jest zarezerwowana od {conflictingReservation.From:d} do {conflictingReservation.To:d}");
        }

        item.Reserve(userEmail, from, to);
        var reservation = new Reservation(_nextReservationId++, item, userEmail, from, to);
        _reservations.Add(reservation);

        OnNewReservation?.Invoke(reservation);
        return reservation;
    }

    public void CancelReservation(int reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
        {
            throw new ArgumentException("Rezerwacji nie odnaleziono");
        }
        reservation.Cancel();
        OnReservationCancelled?.Invoke(reservation);
    }

    public IEnumerable<Reservation> GetUserReservations(string userEmail)
    {
        return _reservations.Where(r => r.Email == userEmail && r.IsActive);
    }
    
    public IEnumerable<Reservation> GetAllReservations() => _reservations.AsReadOnly();
    public IEnumerable<LibraryItem> GetAllItems() => _items.AsReadOnly();
}