using APBD_tutorial12.DTOs;
using APBD_tutorial12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_tutorial12.Controllers;

[Route("api/trips")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<ActionResult> GetTrips()
    {
        var trips = await _tripsService.GetTripAsync();
        return Ok(trips);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<ActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientTripDTO clientTrip)
    {
        var result = await _tripsService.AssignClientToTripAsync(idTrip, clientTrip);
        if (result == -1) return BadRequest("Client already exists");
        if (result == -2) return BadRequest("Trip already happened or doesn't exist");
        if (result == -3) return Conflict("Client already registered to this trip");
        return Ok("Client assigned successfully to the trip");
    }
}