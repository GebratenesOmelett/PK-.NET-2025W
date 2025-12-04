namespace lab3.Models;

public class Van : Vehicle
{
    public double CargoVolume { get; set; }

    public override string GetInfo()
    {
        return $"Van {RegistrationNumber}, ilość: {MaxLoadKg}kg, objętość: {CargoVolume}m³";
    }
}