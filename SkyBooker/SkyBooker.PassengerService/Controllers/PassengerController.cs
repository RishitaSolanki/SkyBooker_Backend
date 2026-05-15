using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.PassengerService.DTOs;
using SkyBooker.PassengerService.Services;

namespace SkyBooker.PassengerService.Controllers;

[ApiController]
[Route("api/Passenger")]
public class PassengerController : ControllerBase
{
    private readonly IPassengerService _passengerService;

    public PassengerController(IPassengerService passengerService)
    {
        _passengerService = passengerService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPassengerById(string id)
    {
        var result = await _passengerService.GetPassengerById(id);
        if (result == null)
            return NotFound(new { message = "Passenger not found" });
        return Ok(result);
    }

    [HttpGet("booking/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> GetPassengersByBooking(string bookingId)
    {
        var result = await _passengerService.GetPassengersByBooking(bookingId);
        return Ok(result);
    }

    [HttpGet("passport/{passportNumber}")]
    [Authorize]
    public async Task<IActionResult> GetByPassportNumber(string passportNumber)
    {
        var result = await _passengerService.GetByPassportNumber(passportNumber);
        if (result == null)
            return NotFound(new { message = "Passenger not found" });
        return Ok(result);
    }

    [HttpGet("ticket/{ticketNumber}")]
    [Authorize]
    public async Task<IActionResult> GetByTicketNumber(string ticketNumber)
    {
        var result = await _passengerService.GetByTicketNumber(ticketNumber);
        if (result == null)
            return NotFound(new { message = "Passenger not found" });
        return Ok(result);
    }

    [HttpGet("seat/{seatId}")]
    [Authorize]
    public async Task<IActionResult> GetBySeatId(string seatId)
    {
        var result = await _passengerService.GetBySeatId(seatId);
        if (result == null)
            return NotFound(new { message = "Passenger not found" });
        return Ok(result);
    }

    [HttpGet("count/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> GetPassengerCount(string bookingId)
    {
        var result = await _passengerService.GetPassengerCount(bookingId);
        return Ok(new { count = result });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPassenger([FromBody] CreatePassengerDto request)
    {
        var result = await _passengerService.AddPassenger(request);
        if (result == null)
            return BadRequest(new { message = "Failed to add passenger. Invalid data." });
        return CreatedAtAction(nameof(GetPassengerById), new { id = result.PassengerId }, result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdatePassenger(string id, [FromBody] CreatePassengerDto request)
    {
        var result = await _passengerService.UpdatePassenger(id, request);
        if (result == null)
            return NotFound(new { message = "Passenger not found or invalid data" });
        return Ok(result);
    }

    [HttpPut("{id}/assign-seat")]
    [Authorize]
    public async Task<IActionResult> AssignSeat(string id, [FromBody] AssignSeatDto request)
    {
        var result = await _passengerService.AssignSeat(id, request.SeatId, request.SeatNumber);
        if (result == null)
            return NotFound(new { message = "Passenger not found" });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePassenger(string id)
    {
        var result = await _passengerService.DeletePassenger(id);
        if (!result)
            return NotFound(new { message = "Passenger not found" });
        return Ok(new { message = "Passenger deleted successfully" });
    }

    [HttpDelete("booking/{bookingId}")]
    [Authorize(Roles = "ADMIN, AIRLINE_STAFF")]
    public async Task<IActionResult> DeletePassengersByBooking(string bookingId)
    {
        var result = await _passengerService.DeletePassengersByBooking(bookingId);
        if (!result)
            return NotFound(new { message = "No passengers found for this booking" });
        return Ok(new { message = "Passengers deleted successfully" });
    }
}

public class AssignSeatDto
{
    public string SeatId { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
}
