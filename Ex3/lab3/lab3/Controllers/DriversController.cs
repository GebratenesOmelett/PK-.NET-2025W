using lab3.Data;
using lab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly FleetDbContext _context;

    public DriversController(FleetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
    {
        return await _context.Drivers.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Driver>> CreateDriver(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDrivers), new { id = driver.Id }, driver);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Driver>>> GetAvailableDrivers()
    {
        return await _context.Drivers.Where(d => d.IsAvailable).ToListAsync();
    }
}