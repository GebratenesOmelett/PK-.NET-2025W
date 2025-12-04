namespace lab3.Models;

public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    
    public List<TransportOrder> Orders { get; set; } = new();
}