using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.AirlineService.Services;
using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Controllers;

[ApiController]
[Route("api/airlines")]
public class AirlineController : ControllerBase
{
    private readonly IAirlineService _airlineService;

    public AirlineController(IAirlineService airlineService)
    {
        _airlineService = airlineService;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateAirline([FromBody] CreateAirlineDto request)
    {
        var airline = new Airline
        {
            Name = request.Name,
            IataCode = request.IataCode,
            IcaoCode = request.IcaoCode,
            LogoUrl = request.LogoUrl,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            IsActive = true
        };

        var result = await _airlineService.CreateAirline(airline);
        return CreatedAtAction(nameof(GetAirlineById), new { id = result.AirlineId }, result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetAirlineById(string id)
    {
        var result = await _airlineService.GetAirlineById(id);
        if (result == null)
            return NotFound(new { message = "Airline not found" });
        return Ok(result);
    }

    [HttpGet("iata/{iataCode}")]
    [Authorize]
    public async Task<IActionResult> GetAirlineByIataCode(string iataCode)
    {
        var result = await _airlineService.GetAirlineByIataCode(iataCode);
        if (result == null)
            return NotFound(new { message = "Airline not found" });
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllAirlines()
    {
        var result = await _airlineService.GetAllAirlines();
        return Ok(result);
    }

    [HttpGet("active")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetActiveAirlines()
    {
        var result = await _airlineService.GetActiveAirlines();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateAirline(string id, [FromBody] UpdateAirlineDto request)
    {
        var existing = await _airlineService.GetAirlineById(id);
        if (existing == null)
            return NotFound(new { message = "Airline not found" });

        existing.Name = request.Name;
        existing.LogoUrl = request.LogoUrl;
        existing.Country = request.Country;
        existing.ContactEmail = request.ContactEmail;
        existing.ContactPhone = request.ContactPhone;

        var result = await _airlineService.UpdateAirline(existing);
        return Ok(result);
    }

    [HttpPut("{id}/deactivate")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeactivateAirline(string id)
    {
        var result = await _airlineService.DeactivateAirline(id);
        if (!result)
            return NotFound(new { message = "Airline not found" });
        return Ok(new { message = "Airline deactivated successfully" });
    }

    // Airport endpoints
    [HttpPost("airports")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateAirport([FromBody] CreateAirportDto request)
    {
        var airport = new Airport
        {
            Name = request.Name,
            IataCode = request.IataCode,
            IcaoCode = request.IcaoCode,
            City = request.City,
            Country = request.Country,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Timezone = request.Timezone
        };

        var result = await _airlineService.CreateAirport(airport);
        return CreatedAtAction(nameof(GetAirportByIataCode), new { iataCode = result.IataCode }, result);
    }

    [HttpGet("airports/{iataCode}")]
    [Authorize]
    public async Task<IActionResult> GetAirportByIataCode(string iataCode)
    {
        var result = await _airlineService.GetAirportByIataCode(iataCode);
        if (result == null)
            return NotFound(new { message = "Airport not found" });
        return Ok(result);
    }

    [HttpGet("airports/city/{city}")]
    [Authorize]
    public async Task<IActionResult> GetAirportsByCity(string city)
    {
        var result = await _airlineService.GetAirportsByCity(city);
        return Ok(result);
    }

    [HttpGet("airports/country/{country}")]
    [Authorize]
    public async Task<IActionResult> GetAirportsByCountry(string country)
    {
        var result = await _airlineService.GetAirportsByCountry(country);
        return Ok(result);
    }

    [HttpGet("airports/search")]
    [Authorize]
    public async Task<IActionResult> SearchAirports([FromQuery] string searchTerm)
    {
        var result = await _airlineService.SearchAirports(searchTerm);
        return Ok(result);
    }

    [HttpPut("airports/{iataCode}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateAirport(string iataCode, [FromBody] UpdateAirportDto request)
    {
        var existing = await _airlineService.GetAirportByIataCode(iataCode);
        if (existing == null)
            return NotFound(new { message = "Airport not found" });

        existing.Name = request.Name;
        existing.City = request.City;
        existing.Country = request.Country;
        existing.Latitude = request.Latitude;
        existing.Longitude = request.Longitude;
        existing.Timezone = request.Timezone;

        var result = await _airlineService.UpdateAirport(existing);
        return Ok(result);
    }
}

public class CreateAirlineDto
{
    public string Name { get; set; } = string.Empty;
    public string IataCode { get; set; } = string.Empty;
    public string IcaoCode { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
}

public class UpdateAirlineDto
{
    public string Name { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
}

public class CreateAirportDto
{
    public string Name { get; set; } = string.Empty;
    public string IataCode { get; set; } = string.Empty;
    public string IcaoCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
}

public class UpdateAirportDto
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Timezone { get; set; } = string.Empty;
}
