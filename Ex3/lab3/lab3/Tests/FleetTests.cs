using lab3.Data;
using lab3.Extensions;
using lab3.Models;
using lab3.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace lab3.Tests;

[TestFixture]
public class FleetTests
{
    [Test]
    public void Truck_GetInfo_ReturnsCorrectInfo()
    {

        var truck = new Truck 
        { 
            RegistrationNumber = "TEST123", 
            MaxLoadKg = 20000, 
            TrailerLength = 12.5 
        };
        
        var info = truck.GetInfo();
        
        Assert.That(info, Does.Contain("Ciężarówka TEST123"));
        Assert.That(info, Does.Contain("20000"));
        Assert.That(info, Does.Contain("12.5"));
    }

    [Test]
    public void VehicleExtensions_GetAvailableVehicles_ReturnsOnlyAvailable()
    {
        var vehicles = new List<Vehicle>
        {
            new Truck { Id = 1, IsAvailable = true },
            new Truck { Id = 2, IsAvailable = false },
            new Van { Id = 3, IsAvailable = true }
        };
        
        var available = vehicles.GetAvailableVehicles().ToList();
        
        Assert.That(available, Has.Count.EqualTo(2));
        Assert.That(available.All(v => v.IsAvailable), Is.True);
    }

    [Test]
    public void TransportOrder_CompleteOrder_SetsPropertiesCorrectly()
    {
        var vehicle = new Truck { IsAvailable = false };
        var driver = new Driver { IsAvailable = false };
        var order = new TransportOrder 
        { 
            Vehicle = vehicle, 
            Driver = driver, 
            IsCompleted = false 
        };
        
        order.CompleteOrder();
        
        Assert.That(order.IsCompleted, Is.True);
        Assert.That(vehicle.IsAvailable, Is.True);
        Assert.That(driver.IsAvailable, Is.True);
    }

    [Test]
    public void FleetManager_OnNewOrderCreated_EventFires()
    {
        var fleetManager = new FleetManager();
        string? eventMessage = null;
        fleetManager.OnNewOrderCreated += (message) => eventMessage = message;

        fleetManager.NotifyNewOrderCreated("Test message");

        Assert.That(eventMessage, Is.EqualTo("Test message"));
    }

    [Test]
    public async Task Database_CanCreateAndRetrieveVehicles()
    {
        var options = new DbContextOptionsBuilder<FleetDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new FleetDbContext(options))
        {
            var truck = new Truck { RegistrationNumber = "TEST456", MaxLoadKg = 15000 };
            context.Trucks.Add(truck);
            await context.SaveChangesAsync();
        }

        using (var context = new FleetDbContext(options))
        {
            var savedTruck = await context.Trucks.FirstAsync();
            Assert.That(savedTruck.RegistrationNumber, Is.EqualTo("TEST456"));
        }
    }
}