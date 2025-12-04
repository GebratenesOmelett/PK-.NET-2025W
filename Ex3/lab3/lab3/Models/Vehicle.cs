namespace lab3.Models;

public abstract class Vehicle
{
    public int Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public double MaxLoadKg { get; set; }
    public bool IsAvailable { get; set; } = true;

    public abstract string GetInfo();
}