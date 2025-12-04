using lab3.Data;
using lab3.Models;
using lab3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly FleetDbContext _context;
    private readonly FleetManager _fleetManager;

    public OrdersController(FleetDbContext context, FleetManager fleetManager)
    {
        _context = context;
        _fleetManager = fleetManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetOrders()
    {
        var orders = await _context.TransportOrders
            .Include(o => o.Vehicle)
            .Include(o => o.Driver)
            .Where(o => !o.IsCompleted)
            .ToListAsync();

        return Ok(orders.Select(o => new {
            o.Id,
            o.CargoDescription,
            o.Weight,
            o.CreatedAt,
            o.IsCompleted,
            Vehicle = o.Vehicle?.GetInfo(),
            Driver = o.Driver?.Name
        }));
    }

    [HttpPost]
    public async Task<ActionResult<TransportOrder>> CreateOrder(TransportOrder order)
    {
        var vehicle = await _context.Vehicles.FindAsync(order.VehicleId);
        var driver = await _context.Drivers.FindAsync(order.DriverId);

        if (vehicle == null || driver == null)
            return BadRequest("Pojazd lub kierowca nie istnieje");

        if (!_fleetManager.ValidateOrder(order, vehicle, driver))
            return BadRequest("Nie można utworzyć zlecenia - sprawdź dostępność lub ładowność");

        order.Vehicle = vehicle;
        order.Driver = driver;
        order.StartOrder();

        _context.TransportOrders.Add(order);
        await _context.SaveChangesAsync();

        _fleetManager.NotifyNewOrderCreated(
            $"Utworzono nowe zlecenie: {order.CargoDescription}, " +
            $"Pojazd: {vehicle.GetInfo()}, " +
            $"Kierowca: {driver.Name}");

        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteOrder(int id)
    {
        var order = await _context.TransportOrders
            .Include(o => o.Vehicle)
            .Include(o => o.Driver)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        order.CompleteOrder();
        await _context.SaveChangesAsync();

        return NoContent();
    }
}