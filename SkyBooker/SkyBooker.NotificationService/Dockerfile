FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SkyBooker/SkyBooker.NotificationService/SkyBooker.NotificationService.csproj"
RUN dotnet publish "SkyBooker/SkyBooker.NotificationService/SkyBooker.NotificationService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SkyBooker.NotificationService.dll"]