using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Services;

namespace SkyBooker.BookingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto request)
    {
        var result = await _bookingService.CreateBooking(request);
        if (result == null)
            return BadRequest(new { message = "Failed to create booking" });
        return CreatedAtAction(nameof(GetBookingById), new { id = result.BookingId }, result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetBookingById(string id)
    {
        var result = await _bookingService.GetBookingById(id);
        if (result == null)
            return NotFound(new { message = "Booking not found" });
        return Ok(result);
    }

    [HttpGet("pnr/{pnrCode}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookingByPnr(string pnrCode)
    {
        var result = await _bookingService.GetBookingByPnr(pnrCode);
        if (result == null)
            return NotFound(new { message = "Booking not found" });
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetBookingsByUser(string userId)
    {
        var result = await _bookingService.GetBookingsByUser(userId);
        return Ok(result);
    }

    [HttpGet("flight/{flightId}")]
    [Authorize(Roles = "ADMIN,AIRLINE_STAFF")]
    public async Task<IActionResult> GetBookingsByFlight(int flightId)
    {
        var result = await _bookingService.GetBookingsByFlight(flightId);
        return Ok(result);
    }

    [HttpGet("user/{userId}/upcoming")]
    [Authorize]
    public async Task<IActionResult> GetUpcomingBookings(string userId)
    {
        var result = await _bookingService.GetUpcomingBookings(userId);
        return Ok(result);
    }

    [HttpPut("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelBooking(string id)
    {
        var result = await _bookingService.CancelBooking(id);
        if (!result)
            return NotFound(new { message = "Booking not found" });
        return Ok(new { message = "Booking cancelled successfully" });
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "ADMIN,AIRLINE_STAFF")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] string status)
    {
        var result = await _bookingService.UpdateStatus(id, status);
        if (!result)
            return NotFound(new { message = "Booking not found" });
        return Ok(new { message = "Booking status updated successfully" });
    }

    [HttpPost("calculate-fare")]
    [Authorize]
    public async Task<IActionResult> CalculateFare([FromBody] CreateBookingDto request)
    {
        var result = await _bookingService.CalculateFare(request);
        return Ok(result);
    }
}
