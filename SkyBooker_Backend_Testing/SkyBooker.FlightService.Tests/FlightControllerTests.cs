using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SkyBooker.FlightService.Common;
using SkyBooker.FlightService.Controllers;
using SkyBooker.FlightService.DTOs;
using SkyBooker.FlightService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyBooker.FlightService.Tests
{
    [TestFixture]
    public class FlightControllerTests
    {
        private Mock<IFlightService> _mockFlightService;
        private Mock<ILogger<FlightController>> _mockLogger;
        private FlightController _controller;

        [SetUp]
        public void Setup()
        {
            _mockFlightService = new Mock<IFlightService>();
            _mockLogger = new Mock<ILogger<FlightController>>();
            _controller = new FlightController(_mockFlightService.Object, _mockLogger.Object);
        }

        // --- 1. GetFlightById ---
        [Test]
        public async Task GetFlightById_ReturnsOkResult_WhenFlightExists()
        {
            var flightId = 1;
            var flightDto = new FlightDto { FlightId = flightId, FlightNumber = "FL123" };
            var apiResponse = ApiResponse<FlightDto>.Success(flightDto, "Success");

            _mockFlightService.Setup(s => s.GetFlightByIdAsync(flightId)).ReturnsAsync(apiResponse);

            var result = await _controller.GetFlightById(flightId);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        // --- 2. GetFlightByNumber ---
        [Test]
        public async Task GetFlightByNumber_ReturnsOkResult_WhenFlightExists()
        {
            var flightNum = "FL123";
            var flightDto = new FlightDto { FlightId = 1, FlightNumber = flightNum };
            var apiResponse = ApiResponse<FlightDto>.Success(flightDto, "Success");

            _mockFlightService.Setup(s => s.GetFlightByNumberAsync(flightNum)).ReturnsAsync(apiResponse);

            var result = await _controller.GetFlightByNumber(flightNum);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetFlightByNumber_ReturnsNotFound_WhenFlightDoesNotExist()
        {
            var flightNum = "FL999";
            var apiResponse = ApiResponse<FlightDto>.Failure("Not Found", 404);

            _mockFlightService.Setup(s => s.GetFlightByNumberAsync(flightNum)).ReturnsAsync(apiResponse);

            var result = await _controller.GetFlightByNumber(flightNum);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(404));
        }

        // --- 3. GetAllFlights ---
        [Test]
        public async Task GetAllFlights_ReturnsOkResult_WithListOfFlights()
        {
            var flights = new List<FlightDto> { new FlightDto { FlightId = 1, FlightNumber = "FL123" } };
            var apiResponse = ApiResponse<IEnumerable<FlightDto>>.Success(flights, "Success");

            _mockFlightService.Setup(s => s.GetAllFlightsAsync()).ReturnsAsync(apiResponse);

            var result = await _controller.GetAllFlights();
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        // --- 4. SearchFlights (One-way) ---
        [Test]
        public async Task SearchFlights_ReturnsOkResult_WhenParametersAreValid()
        {
            var searchDto = new SearchFlightDto();
            var flights = new List<FlightDto> { new FlightDto { FlightId = 1, FlightNumber = "FL123" } };
            var apiResponse = ApiResponse<IEnumerable<FlightDto>>.Success(flights, "Success");

            _mockFlightService.Setup(s => s.SearchFlightsAsync(searchDto)).ReturnsAsync(apiResponse);

            var result = await _controller.SearchFlights(searchDto);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task SearchFlights_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var searchDto = new SearchFlightDto();
            _controller.ModelState.AddModelError("OriginAirportCode", "Required");

            var result = await _controller.SearchFlights(searchDto);
            var objectResult = result as BadRequestObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(400));
        }

        // --- 5. SearchRoundTrip ---
        [Test]
        public async Task SearchRoundTrip_ReturnsOkResult_WhenDatesAreValid()
        {
            var searchDto = new RoundTripSearchDto 
            { 
                DepartureDate = DateTime.Now.AddDays(1), 
                ReturnDate = DateTime.Now.AddDays(5) 
            };
            var roundTripData = new Dictionary<string, IEnumerable<FlightDto>>();
            var apiResponse = ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>.Success(roundTripData, "Success");

            _mockFlightService.Setup(s => s.SearchRoundTripAsync(searchDto)).ReturnsAsync(apiResponse);

            var result = await _controller.SearchRoundTrip(searchDto);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task SearchRoundTrip_ReturnsBadRequest_WhenReturnDateIsBeforeDeparture()
        {
            var searchDto = new RoundTripSearchDto 
            { 
                DepartureDate = DateTime.Now.AddDays(5), 
                ReturnDate = DateTime.Now.AddDays(1) 
            };

            var result = await _controller.SearchRoundTrip(searchDto);
            var objectResult = result as BadRequestObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(400));
        }

        // --- 6. GetFlightsByAirline ---
        [Test]
        public async Task GetFlightsByAirline_ReturnsOkResult_WhenAirlineExists()
        {
            var airlineId = 1;
            var flights = new List<FlightDto> { new FlightDto { FlightId = 1, FlightNumber = "FL123" } };
            var apiResponse = ApiResponse<IEnumerable<FlightDto>>.Success(flights, "Success");

            _mockFlightService.Setup(s => s.GetFlightsByAirlineAsync(airlineId)).ReturnsAsync(apiResponse);

            var result = await _controller.GetFlightsByAirline(airlineId);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        // --- 7. CreateFlight ---
        [Test]
        public async Task CreateFlight_ReturnsCreatedResult_WhenModelIsValid()
        {
            var createDto = new CreateFlightDto { FlightNumber = "FL789" };
            var flightDto = new FlightDto { FlightId = 3, FlightNumber = "FL789" };
            var apiResponse = ApiResponse<FlightDto>.Success(flightDto, "Created", 201);

            _mockFlightService.Setup(s => s.CreateFlightAsync(createDto)).ReturnsAsync(apiResponse);

            var result = await _controller.CreateFlight(createDto);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task CreateFlight_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var createDto = new CreateFlightDto();
            _controller.ModelState.AddModelError("FlightNumber", "Required");

            var result = await _controller.CreateFlight(createDto);
            var objectResult = result as BadRequestObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(400));
        }

        // --- 8. UpdateFlight ---
        [Test]
        public async Task UpdateFlight_ReturnsOkResult_WhenSuccessful()
        {
            var flightId = 1;
            var updateDto = new UpdateFlightDto();
            var flightDto = new FlightDto { FlightId = flightId, FlightNumber = "FL123-Updated" };
            var apiResponse = ApiResponse<FlightDto>.Success(flightDto, "Updated", 200);

            _mockFlightService.Setup(s => s.UpdateFlightAsync(flightId, updateDto)).ReturnsAsync(apiResponse);

            var result = await _controller.UpdateFlight(flightId, updateDto);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task UpdateFlight_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var flightId = 1;
            var updateDto = new UpdateFlightDto();
            _controller.ModelState.AddModelError("AircraftType", "Required");

            var result = await _controller.UpdateFlight(flightId, updateDto);
            var objectResult = result as BadRequestObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(400));
        }

        // --- 9. UpdateFlightStatus ---
        [Test]
        public async Task UpdateFlightStatus_ReturnsOkResult_WhenSuccessful()
        {
            var flightId = 1;
            var status = "Delayed";
            var apiResponse = ApiResponse<bool>.Success(true, "Status updated");

            _mockFlightService.Setup(s => s.UpdateFlightStatusAsync(flightId, status)).ReturnsAsync(apiResponse);

            var result = await _controller.UpdateFlightStatus(flightId, status);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        // --- 10. DeleteFlight ---
        [Test]
        public async Task DeleteFlight_ReturnsOkResult_WhenSuccessful()
        {
            var flightId = 1;
            var apiResponse = ApiResponse<bool>.Success(true, "Flight deleted");

            _mockFlightService.Setup(s => s.DeleteFlightAsync(flightId)).ReturnsAsync(apiResponse);

            var result = await _controller.DeleteFlight(flightId);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        // --- 11. DecrementSeats ---
        [Test]
        public async Task DecrementSeats_ReturnsOkResult_WhenSuccessful()
        {
            var flightId = 1;
            var count = 2;
            var apiResponse = ApiResponse<bool>.Success(true, "Seats decremented");

            _mockFlightService.Setup(s => s.DecrementSeatsAsync(flightId, count)).ReturnsAsync(apiResponse);

            var result = await _controller.DecrementSeats(flightId, count);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }

        // --- 12. IncrementSeats ---
        [Test]
        public async Task IncrementSeats_ReturnsOkResult_WhenSuccessful()
        {
            var flightId = 1;
            var count = 2;
            var apiResponse = ApiResponse<bool>.Success(true, "Seats incremented");

            _mockFlightService.Setup(s => s.IncrementSeatsAsync(flightId, count)).ReturnsAsync(apiResponse);

            var result = await _controller.IncrementSeats(flightId, count);
            var objectResult = result as ObjectResult;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        }
    }
}
