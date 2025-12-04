using lab3.Models;

namespace lab3.Extensions;

public static class VehicleExtensions
{
    public static IEnumerable<Vehicle> GetAvailableVehicles(this IEnumerable<Vehicle> vehicles)
    {
        return vehicles.Where(v => v.IsAvailable);
    }

    public static IEnumerable<Vehicle> GetVehiclesByLoadCapacity(this IEnumerable<Vehicle> vehicles, double minCapacity)
    {
        return vehicles.Where(v => v.MaxLoadKg >= minCapacity);
    }
}