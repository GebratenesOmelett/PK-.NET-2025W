using lab3.Data;
using lab3.Extensions;
using lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly FleetDbContext _context;

    public VehiclesController(FleetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetVehicles()
    {
        var trucks = await _context.Trucks.ToListAsync();
        var vans = await _context.Vans.ToListAsync();
        
        var allVehicles = trucks.Cast<Vehicle>().Concat(vans.Cast<Vehicle>());
        return Ok(allVehicles.Select(v => new {
            v.Id,
            v.RegistrationNumber,
            v.MaxLoadKg,
            v.IsAvailable,
            Type = v.GetType().Name,
            Info = v.GetInfo()
        }));
    }

    [HttpPost("truck")]
    public async Task<ActionResult<Truck>> CreateTruck(Truck truck)
    {
        _context.Trucks.Add(truck);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetVehicles), new { id = truck.Id }, truck);
    }

    [HttpPost("van")]
    public async Task<ActionResult<Van>> CreateVan(Van van)
    {
        _context.Vans.Add(van);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetVehicles), new { id = van.Id }, van);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles()
    {
        var trucks = await _context.Trucks.ToListAsync();
        var vans = await _context.Vans.ToListAsync();
        
        var allVehicles = trucks.Cast<Vehicle>().Concat(vans.Cast<Vehicle>());
        return Ok(allVehicles.GetAvailableVehicles());
    }
}