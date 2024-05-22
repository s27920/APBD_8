using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Context;
using WebApplication4.Dtos;
using WebApplication4.Models;

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
        return Ok(await _context.Trips.OrderByDescending(t=>t.DateFrom).ToListAsync());
    }

    [HttpDelete("clients/{id:int}")]
    public async Task<IActionResult> DeletebyIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client is null)
        {
            return NotFound("No client with given id exists");
        }

        var trips = await _context.Clients.Where(c => c.IdClient == id).SelectMany(c => c.ClientTrips).CountAsync();
        if (trips > 0)
        {
            return Conflict("Can't delete client with registered trips");
        }
        return Ok(await _context.Clients.Where(c => c.IdClient == id).ExecuteDeleteAsync() > 0);
    }

    [HttpPost(("trips/{id:int}/clients"))]
    public async Task<IActionResult> AssignClientToTripAsync([FromBody] ClientTripDto dto, int id)
    {
        var client = await _context.Clients.FindAsync(dto.Pesel);
        if (client is null)
        {
            client = new Client()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                IdClient = dto.IdClient,
                ClientTrips = new List<ClientTrip>(),
                Pesel = dto.Pesel,
                Telephone = dto.Telephone
            };
            await _context.Clients.AddAsync(client);
        }
        var inTrip = await _context.ClientTrips.Where(c=>dto.IdClient == c.IdClient).Where(c=>c.IdTrip == id).ToListAsync();
        if (inTrip.Count > 0)
        {
            return Conflict("Client already registered for given trip");
        }
        
        var trip = await _context.Trips.FindAsync(id);
        if (trip is null)
        {
            return NotFound("No trip with given id exists");
        }
        DateTime now = DateTime.Now;
        await _context.ClientTrips.AddAsync(new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = trip.IdTrip,
            PaymentDate = DateTime.Parse(dto.PaymentTime),
            RegisteredAt = now
        });
        return Ok("inserted");
    }
}