using System.Text.Json.Serialization;

namespace lab3.Models;

public class TransportOrder : IReservable
{
    public int Id { get; set; }
    public string CargoDescription { get; set; } = string.Empty;
    public double Weight { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; } = false;

    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public int DriverId { get; set; }
    [JsonIgnore]
    public Driver? Driver { get; set; }

    public void AssignDriver(Driver driver)
    {
        Driver = driver;
        Driver.IsAvailable = false;
    }

    public void StartOrder()
    {
        if (Vehicle != null)
        {
            Vehicle.IsAvailable = false;
        }

        if (Driver != null)
        {
            Driver.IsAvailable = false;
        }
    }

    public void CompleteOrder()
    {
        IsCompleted = true;
        if (Vehicle != null)
        {
            Vehicle.IsAvailable = true;
        }

        if (Driver != null)
        {
            Driver.IsAvailable = true;
        }
    }
}