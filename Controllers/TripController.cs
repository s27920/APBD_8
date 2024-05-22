using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Context;
using WebApplication4.Dtos;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly Apbd8Context _context;

    public TripController(Apbd8Context context)
    {
        _context = context;
    }

    [HttpGet("trips")]
    public async Task<IActionResult> GetTripsAsync()
    {
        return Ok(await _context.Trips.OrderBy(t=>t.DateFrom).ToListAsync());
    }

    [HttpDelete("clients/{id:int}")]
    public async Task<IActionResult> DeletebyIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is null)
        {
            return NotFound("No client with such given id exists");
        }

        return Ok(await _context.Clients.Where(c => c.IdClient == id).ExecuteDeleteAsync() > 0);
    }

    [HttpPost(("trips/{id:int}/clients"))]
    public async Task<IActionResult> assignClientToTripAsync([FromBody] ClientTripDto dto)
    {
        
    }
}