using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using SkyBooker.FlightService.Common;

using SkyBooker.FlightService.DTOs;

using SkyBooker.FlightService.Services.Interfaces;



namespace SkyBooker.FlightService.Controllers;



[ApiController]

[Route("api/[controller]")]

public class FlightController : ControllerBase

{

    private readonly IFlightService _flightService;

    private readonly ILogger<FlightController> _logger;



    public FlightController(IFlightService flightService, ILogger<FlightController> logger)

    {

        _flightService = flightService;

        _logger = logger;

    }



    /// <summary>

    /// Get flight by ID

    /// </summary>

    [HttpGet("{id}")]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 200)]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 404)]

    public async Task<IActionResult> GetFlightById(int id)

    {

        var response = await _flightService.GetFlightByIdAsync(id);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Get flight by flight number

    /// </summary>

    [HttpGet("number/{flightNumber}")]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 200)]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 404)]

    public async Task<IActionResult> GetFlightByNumber(string flightNumber)

    {

        var response = await _flightService.GetFlightByNumberAsync(flightNumber);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Get all flights

    /// </summary>

    [HttpGet]

    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FlightDto>>), 200)]

    public async Task<IActionResult> GetAllFlights()

    {

        var response = await _flightService.GetAllFlightsAsync();

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Search one-way flights

    /// </summary>

    [HttpPost("search/oneway")]

    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FlightDto>>), 200)]

    public async Task<IActionResult> SearchFlights([FromBody] SearchFlightDto searchDto)

    {

        if (!ModelState.IsValid)

        {

            return BadRequest(ApiResponse<IEnumerable<FlightDto>>.Failure("Invalid search parameters", 400));

        }



        var response = await _flightService.SearchFlightsAsync(searchDto);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Search round-trip flights

    /// </summary>

    [HttpPost("search/roundtrip")]

    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>), 200)]

    public async Task<IActionResult> SearchRoundTrip([FromBody] RoundTripSearchDto searchDto)

    {

        if (!ModelState.IsValid)

        {

            return BadRequest(ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>.Failure("Invalid search parameters", 400));

        }



        if (searchDto.ReturnDate <= searchDto.DepartureDate)

        {

            return BadRequest(ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>.Failure(

                "Return date must be after departure date", 400));

        }



        var response = await _flightService.SearchRoundTripAsync(searchDto);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Get flights by airline

    /// </summary>

    [HttpGet("airline/{airlineId}")]

    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FlightDto>>), 200)]

    public async Task<IActionResult> GetFlightsByAirline(int airlineId)

    {

        var response = await _flightService.GetFlightsByAirlineAsync(airlineId);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Create a new flight (Airline Staff/Admin only)

    /// </summary>

    [HttpPost]

    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 201)]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 400)]

    public async Task<IActionResult> CreateFlight([FromBody] CreateFlightDto createDto)

    {

        if (!ModelState.IsValid)

        {

            return BadRequest(ApiResponse<FlightDto>.Failure("Invalid flight data", 400));

        }



        var response = await _flightService.CreateFlightAsync(createDto);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Update flight details (Airline Staff/Admin only)

    /// </summary>

    [HttpPut("{id}")]

    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 200)]

    [ProducesResponseType(typeof(ApiResponse<FlightDto>), 404)]

    public async Task<IActionResult> UpdateFlight(int id, [FromBody] UpdateFlightDto updateDto)

    {

        if (!ModelState.IsValid)

        {

            return BadRequest(ApiResponse<FlightDto>.Failure("Invalid flight data", 400));

        }



        var response = await _flightService.UpdateFlightAsync(id, updateDto);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Update flight status (Airline Staff/Admin only)

    /// </summary>

    [HttpPatch("{id}/status")]

    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]

    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]

    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]

    public async Task<IActionResult> UpdateFlightStatus(int id, [FromBody] string status)

    {

        var response = await _flightService.UpdateFlightStatusAsync(id, status);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Delete flight (Admin only)

    /// </summary>

    [HttpDelete("{id}")]

    [Authorize(Roles = "ADMIN")]

    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]

    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]

    public async Task<IActionResult> DeleteFlight(int id)

    {

        var response = await _flightService.DeleteFlightAsync(id);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Decrement available seats (Internal use - Booking Service)

    /// </summary>

    [HttpPatch("{id}/seats/decrement")]

    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]

    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]

    public async Task<IActionResult> DecrementSeats(int id, [FromQuery] int count = 1)

    {

        var response = await _flightService.DecrementSeatsAsync(id, count);

        return StatusCode(response.StatusCode, response);

    }



    /// <summary>

    /// Increment available seats (Internal use - Cancellation)

    /// </summary>

    [HttpPatch("{id}/seats/increment")]

    [Authorize(Roles = "AIRLINE_STAFF,ADMIN")]

    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]

    public async Task<IActionResult> IncrementSeats(int id, [FromQuery] int count = 1)

    {

        var response = await _flightService.IncrementSeatsAsync(id, count);

        return StatusCode(response.StatusCode, response);

    }

}