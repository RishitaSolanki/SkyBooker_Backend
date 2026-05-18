# SkyBooker Backend ✈

Welcome to the **SkyBooker Backend** codebase! This repository hosts the microservices architecture powering the SkyBooker flight booking platform.

---

## 🏗 System Architecture & Testing Directory Structure

To keep production builds lean and clean, the testing configurations, mocks, and test assertions are fully decoupled from production directories and isolated within `SkyBooker_Backend_Testing`.

```mermaid
graph TD
    subgraph "SkyBooker Backend Workspace"
        direction TB
        B_Src["SkyBooker Source Code"]
        subgraph "d:\SkyBooker_Backend\SkyBooker"
            Auth["SkyBooker.AuthService"]
            Flight["SkyBooker.FlightService"]
        end
        
        subgraph "d:\SkyBooker_Backend\SkyBooker_Backend_Testing"
            AuthTests["SkyBooker.AuthService.Tests"]
            FlightTests["SkyBooker.FlightService.Tests"]
            Sln["SkyBooker.Tests.sln"]
        end
        
        AuthTests -.->|"References"| Auth
        FlightTests -.->|"References"| Flight
        Sln --> AuthTests
        Sln --> FlightTests
    end
```

---

## 🔄 Service Communication Flow Diagram

The sequence diagram below illustrates the end-to-end service communication during the user transaction lifecycle (Searching, Booking, and Payment Processing).

```mermaid
sequenceDiagram
    autonumber
    actor User as React Frontend
    participant Auth as Auth Microservice
    participant Flight as Flight Microservice
    participant Booking as Booking Microservice
    participant Payment as Payment Microservice
    participant Razorpay as Razorpay API

    User->>Flight: Search Flights (Origin, Destination, Date)
    Flight-->>User: Return available Flight list & Seat Prices

    User->>Auth: User Log in / Authenticate (credentials)
    Auth-->>User: Issue signed JWT Token

    User->>Booking: Create Booking (FlightId, Seat Details, JWT)
    Note over Booking: Instantiates PENDING booking
    Booking->>Flight: Decrement Available Seats (/seats/decrement)
    Flight-->>Booking: Confirm seat allocation
    Booking-->>User: Return Pending Booking details & TotalFare

    User->>Payment: Initiate Payment (BookingId, Amount, JWT)
    Note over Payment: Instantiates PENDING payment record
    Payment->>Razorpay: Generate Razorpay Order ID
    Razorpay-->>Payment: Return Order ID details
    Payment-->>User: Return payment order configs

    User->>Razorpay: Execute checkout SDK in browser
    Razorpay-->>User: Return Payment Signature & Transaction ID

    User->>Payment: Verify Payment Signature (Transaction Details)
    Note over Payment: Updates Payment to COMPLETED
    Payment->>Booking: Confirm Payment Complete (/api/bookings/confirm)
    Note over Booking: Updates Booking status to CONFIRMED
    Booking-->>Payment: Acknowledge status update
    Payment-->>User: Return confirmed transaction status
```

---

## 🗄 Database Entity-Relationship (ER) Diagram

Each microservice manages its own isolated database instance. Below is the conceptual entity-relationship (ER) mapping highlighting logical references across services.

```mermaid
erDiagram
    USER ||--o{ BOOKING : "places"
    FLIGHT ||--o{ BOOKING : "belongs to"
    AIRLINE ||--o{ FLIGHT : "operates"
    BOOKING ||--o| PAYMENT : "has"
    USER ||--o{ PAYMENT : "makes"

    USER {
        Guid UserId PK
        string FullName
        string Email
        string PasswordHash
        string Phone
        string Role
        string PassportNumber
        string Nationality
        DateTime CreatedAt
    }
    FLIGHT {
        int FlightId PK
        string FlightNumber
        int AirlineId FK
        string OriginAirportCode
        string DestinationAirportCode
        DateTime DepartureTime
        DateTime ArrivalTime
        FlightStatus Status
        int TotalSeats
        int AvailableSeats
        decimal BusinessPrice
        decimal EconomyPrice
    }
    AIRLINE {
        int AirlineId PK
        string Name
        string Code
    }
    BOOKING {
        string BookingId PK
        string UserId FK
        int FlightId FK
        string PnrCode
        string TripType
        string Status
        decimal TotalFare
        string MealPreference
        int LuggageKg
        string PaymentId FK
        DateTime BookedAt
    }
    PAYMENT {
        string PaymentId PK
        string BookingId FK
        string UserId FK
        decimal Amount
        string Currency
        string Status
        string PaymentMode
        string TransactionId
        DateTime PaidAt
    }
```

---

## 🧪 Backend Unit Testing

We implement rigorous unit testing using **NUnit**, **Moq** for service dependency mocking, and **Microsoft.EntityFrameworkCore.InMemory** for database isolation.

### UML Class & Interaction Diagram

The diagram below shows how the C# API Controllers, their mocked service interfaces, and the test suites interact.

```mermaid
classDiagram
    class AuthController {
        -IAuthService _authService
        +Register(RegisterRequest) Task~IActionResult~
        +Login(LoginRequest) Task~IActionResult~
        +GetUserById(Guid) Task~IActionResult~
        +GetUserByEmail(string) Task~IActionResult~
        +SearchUsers(string) Task~IActionResult~
        +DeleteUser(Guid) Task~IActionResult~
        +UpdateUser(Guid, UpdateUserRequest) Task~IActionResult~
        +UpdatePassword(Guid, UpdatePasswordRequest) Task~IActionResult~
        +UpdateUserRole(Guid, string) Task~IActionResult~
    }
    
    class AuthControllerTests {
        -Mock~IAuthService~ _mockAuthService
        -AuthController _controller
        +Setup() void
        +Register_ReturnsOkResult_WithAuthResponse() Task
        +Login_ReturnsOkResult_WithAuthResponse() Task
        +GetUserById_ReturnsOkResult_WhenUserExists() Task
        +GetUserById_ReturnsNotFound_WhenUserDoesNotExist() Task
        +GetUserByEmail_ReturnsOkResult() Task
        +SearchUsers_ReturnsOkResult() Task
        +DeleteUser_ReturnsOkResult() Task
        +UpdateUser_ReturnsOkResult() Task
        +UpdatePassword_ReturnsOkResult() Task
        +UpdateUserRole_ReturnsOkResult() Task
    }

    class FlightController {
        -IFlightService _flightService
        -ILogger _logger
        +GetFlightById(int) Task~IActionResult~
        +GetFlightByNumber(string) Task~IActionResult~
        +GetAllFlights() Task~IActionResult~
        +SearchFlights(SearchFlightDto) Task~IActionResult~
        +SearchRoundTrip(RoundTripSearchDto) Task~IActionResult~
        +GetFlightsByAirline(int) Task~IActionResult~
        +CreateFlight(CreateFlightDto) Task~IActionResult~
        +UpdateFlight(int, UpdateFlightDto) Task~IActionResult~
        +UpdateFlightStatus(int, string) Task~IActionResult~
        +DeleteFlight(int) Task~IActionResult~
        +DecrementSeats(int, int) Task~IActionResult~
        +IncrementSeats(int, int) Task~IActionResult~
    }

    class FlightControllerTests {
        -Mock~IFlightService~ _mockFlightService
        -Mock~ILogger~ _mockLogger
        -FlightController _controller
        +Setup() void
        +GetFlightById_ReturnsOkResult() Task
        +GetFlightByNumber_ReturnsOkResult() Task
        +GetAllFlights_ReturnsOkResult() Task
        +SearchFlights_ReturnsOkResult() Task
        +SearchRoundTrip_ReturnsOkResult() Task
        +CreateFlight_ReturnsCreatedResult() Task
        +UpdateFlight_ReturnsOkResult() Task
        +UpdateFlightStatus_ReturnsOkResult() Task
        +DeleteFlight_ReturnsOkResult() Task
    }

    AuthControllerTests --> AuthController : "Tests & Instantiates"
    AuthControllerTests --> Mock_IAuthService : "Injects Mock"
    FlightControllerTests --> FlightController : "Tests & Instantiates"
    FlightControllerTests --> Mock_IFlightService : "Injects Mock"
    
    class Mock_IAuthService {
        <<Interface Mock>>
        Moq setup methods
    }
    class Mock_IFlightService {
        <<Interface Mock>>
        Moq setup methods
    }
```

---

## 🏃 How to Run Backend Unit Tests

You can run the full suite of **33 unit tests** (17 in FlightService, 16 in AuthService) with a single command.

### Prerequisites
Make sure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed on your system.

### Command Execution
1. Navigate into the testing folder:
   ```bash
   cd SkyBooker_Backend_Testing
   ```
2. Execute the test command:
   ```bash
   dotnet test
   ```

You will see output indicating that all 33 tests successfully compile and pass:
```shell
Passed!  - Failed:     0, Passed:    16, Skipped:     0, Total:    16 - SkyBooker.AuthService.Tests.dll
Passed!  - Failed:     0, Passed:    17, Skipped:     0, Total:    17 - SkyBooker.FlightService.Tests.dll
```