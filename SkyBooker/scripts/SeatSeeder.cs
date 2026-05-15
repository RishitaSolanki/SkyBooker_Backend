using Microsoft.Data.Sqlite;
using System;

string dbPath = "../seatservice.db";
using var connection = new SqliteConnection($"Data Source={dbPath}");
connection.Open();

Console.WriteLine("Clearing existing seats...");
var clearCmd = connection.CreateCommand();
clearCmd.CommandText = "DELETE FROM Seats";
clearCmd.ExecuteNonQuery();

Console.WriteLine("Seeding seats...");

// Create seats for flight IDs 1 to 20
for (int flightId = 1; flightId <= 20; flightId++)
{
    Console.WriteLine($"Seeding flight {flightId}...");
    
    // Rows: 1-9 Business (30%), 10-30 Economy (70%)
    for (int row = 1; row <= 30; row++)
    {
        string seatClass = row <= 9 ? "Business" : "Economy";
        decimal multiplier = row <= 9 ? 1.5m : 1.0m;
        
        char[] cols = { 'A', 'B', 'C', 'D', 'E', 'F' };
        for (int c = 0; c < cols.Length; c++)
        {
            string seatNum = $"{row}{cols[c]}";
            bool isWindow = (cols[c] == 'A' || cols[c] == 'F');
            bool isAisle = (cols[c] == 'C' || cols[c] == 'D');
            
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT OR IGNORE INTO Seats (FlightId, SeatNumber, SeatClass, Row, [Column], IsWindow, IsAisle, HasExtraLegroom, Status, PriceMultiplier, CreatedAt)
                VALUES ($flightId, $seatNum, $class, $row, $col, $isWindow, $isAisle, $extra, 'AVAILABLE', $mult, $created)";
            
            cmd.Parameters.AddWithValue("$flightId", flightId);
            cmd.Parameters.AddWithValue("$seatNum", seatNum);
            cmd.Parameters.AddWithValue("$class", seatClass);
            cmd.Parameters.AddWithValue("$row", row);
            cmd.Parameters.AddWithValue("$col", c + 1);
            cmd.Parameters.AddWithValue("$isWindow", isWindow);
            cmd.Parameters.AddWithValue("$isAisle", isAisle);
            cmd.Parameters.AddWithValue("$extra", row == 1 || row == 6); // Extra legroom for first row of each class
            cmd.Parameters.AddWithValue("$mult", multiplier);
            cmd.Parameters.AddWithValue("$created", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            
            cmd.ExecuteNonQuery();
        }
    }
}

Console.WriteLine("Seeding completed!");
connection.Close();
