using lab3.Models;

namespace lab3.Services;

public class FleetManager
{
    public event Action<string>? OnNewOrderCreated;

    public void NotifyNewOrderCreated(string orderInfo)
    {
        OnNewOrderCreated?.Invoke(orderInfo);
    }

    public bool ValidateOrder(TransportOrder order, Vehicle vehicle, Driver driver)
    {
        if (order.Weight > vehicle.MaxLoadKg)
            return false;

        if (!vehicle.IsAvailable || !driver.IsAvailable)
            return false;

        return true;
    }
}