@echo off
echo Starting SkyBooker Backend Services with explicit ports...

:: 1. AuthService (5010)
start "AuthService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.AuthService && dotnet run --urls http://localhost:5010"

:: 2. FlightService (5001)
start "FlightService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.FlightService && dotnet run --urls http://localhost:5001"

:: 3. BookingService (5214)
start "BookingService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.BookingService && dotnet run --urls http://localhost:5214"

:: 4. SeatService (5002)
start "SeatService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.SeatService && dotnet run --urls http://localhost:5002"

:: 5. PassengerService (5290)
start "PassengerService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.PassengerService && dotnet run --urls http://localhost:5290"

:: 6. PaymentService (5087)
start "PaymentService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.PaymentService && dotnet run --urls http://localhost:5087"

:: 7. AirlineService (5008)
start "AirlineService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.AirlineService && dotnet run --urls http://localhost:5008"

:: 8. NotificationService (5006)
start "NotificationService" cmd /k "cd /d D:\SkyBooker_Backend\SkyBooker\SkyBooker.NotificationService && dotnet run --urls http://localhost:5006"

echo All backend services started with explicit ports!
pause
