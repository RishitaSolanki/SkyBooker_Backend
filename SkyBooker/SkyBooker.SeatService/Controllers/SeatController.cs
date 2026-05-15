using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.SeatService.Common;
using SkyBooker.SeatService.DTOs;
using SkyBooker.SeatService.Services;

namespace SkyBooker.SeatService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;
    private readonly ILogger<SeatController> _logger;

    public SeatController(ISeatService seatService, ILogger<SeatController> logger)
    {
        _seatService = seatService;
        _logger = logger;
    }

    /// <summary>
    /// Get seat by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<SeatDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<SeatDto>), 404)]
    public async Task<IActionResult> GetSeatById(int id)
    {
        var response = await _seatService.GetSeatById(id);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Get all seats for a flight
    /// </summary>
    [HttpGet("flight/{flightId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<SeatDto>>), 200)]
    public async Task<IActionResult> GetSeatsByFlightId(int flightId)
    {
        var response = await _seatService.GetSeatsByFlightId(flightId);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Get seats by class for a flight
    /// </summary>
    [HttpGet("flight/{flightId}/class/{seatClass}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<SeatDto>>), 200)]
    public async Task<IActionResult> GetSeatsByClass(int flightId, string seatClass)
    {
        var response = await _seatService.GetSeatsByClass(flightId, seatClass);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Get available seats for a flight
    /// </summary>
    [HttpGet("flight/{flightId}/available")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<SeatDto>>), 200)]
    public async Task<IActionResult> GetAvailableSeats(int flightId)
    {
        var response = await _seatService.GetAvailableSeats(flightId);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Get available seats by class for a flight
    /// </summary>
    [HttpGet("flight/{flightId}/available/class/{seatClass}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<List<SeatDto>>), 200)]
    public async Task<IActionResult> GetAvailableByClass(int flightId, string seatClass)
    {
        var response = await _seatService.GetAvailableByClass(flightId, seatClass);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Count available seats by class for a flight
    /// </summary>
    [HttpGet("flight/{flightId}/count/class/{seatClass}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<int>), 200)]
    public async Task<IActionResult> CountAvailableByClass(int flightId, string seatClass)
    {
        var response = await _seatService.CountAvailableByClass(flightId, seatClass);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Get seat map for a flight (grouped by class)
    /// </summary>
    [HttpGet("flight/{flightId}/map")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, List<SeatDto>>>), 200)]
    public async Task<IActionResult> GetSeatMap(int flightId)
    {
        var response = await _seatService.GetSeatMap(flightId);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Add a new seat
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]
    [ProducesResponseType(typeof(ApiResponse<SeatDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<SeatDto>), 400)]
    public async Task<IActionResult> AddSeat([FromBody] CreateSeatDto createDto)
    {
        var response = await _seatService.AddSeat(createDto);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Update seat details
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]
    [ProducesResponseType(typeof(ApiResponse<SeatDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<SeatDto>), 404)]
    public async Task<IActionResult> UpdateSeat(int id, [FromBody] UpdateSeatDto updateDto)
    {
        var response = await _seatService.UpdateSeat(id, updateDto);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Hold a seat (for booking)
    /// </summary>
    [HttpPatch("{id}/hold")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> HoldSeat(int id)
    {
        var response = await _seatService.HoldSeat(id);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Release a held seat
    /// </summary>
    [HttpPatch("{id}/release")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> ReleaseSeat(int id)
    {
        var response = await _seatService.ReleaseSeat(id);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Confirm a held seat (after payment)
    /// </summary>
    [HttpPatch("{id}/confirm")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> ConfirmSeat(int id)
    {
        var response = await _seatService.ConfirmSeat(id);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Delete a seat
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<IActionResult> DeleteSeat(int id)
    {
        var response = await _seatService.DeleteSeat(id);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Delete all seats for a flight
    /// </summary>
    [HttpDelete("flight/{flightId}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    public async Task<IActionResult> DeleteSeatsForFlight(int flightId)
    {
        var response = await _seatService.DeleteSeatsForFlight(flightId);
        return StatusCode(response.StatusCode, response);
    }
}
