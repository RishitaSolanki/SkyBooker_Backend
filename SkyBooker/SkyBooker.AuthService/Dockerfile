FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "SkyBooker/SkyBooker.AuthService/SkyBooker.AuthService.csproj"
RUN dotnet publish "SkyBooker/SkyBooker.AuthService/SkyBooker.AuthService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SkyBooker.AuthService.dll"]