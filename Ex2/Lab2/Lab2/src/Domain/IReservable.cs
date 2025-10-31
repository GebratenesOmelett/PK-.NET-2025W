namespace Lab2.Domain;

public interface IReservable
{
    void Reserve(string email, DateTime from, DateTime to);
    void CancelReservation(string email);
    bool IsAvailable(string email);
}