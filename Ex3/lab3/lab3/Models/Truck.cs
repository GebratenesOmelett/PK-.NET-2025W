namespace lab3.Models;

public class Truck : Vehicle
{
    public double TrailerLength { get; set; }

    public override string GetInfo()
    {
        return $"Truck {RegistrationNumber}, ilość: {MaxLoadKg}kg, długość naczepy: {TrailerLength}m";
    }
}