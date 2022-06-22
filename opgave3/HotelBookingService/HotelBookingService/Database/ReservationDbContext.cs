using System;
using Consumer.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Consumer.Database;

public class ReservationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            @"Server=" + (Environment.GetEnvironmentVariable("MSSQL_HOST")?? "[::1]") + ","
            + (Environment.GetEnvironmentVariable("MSSQL_PORT")?? "1433") + ";" 
            + "Database=ReservationDb;User Id=SA;Password="
            + (Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "yourStrong(!)Password") + ";"
            + "Trusted_Connection=false;";
        Console.WriteLine(Environment.MachineName + " - " + DateTime.Now.Millisecond + " - "+connectionString);
        optionsBuilder.UseSqlServer(connectionString);
        
        Console.WriteLine(Environment.MachineName + " - " + DateTime.Now.Millisecond +" - connected to db");
    }

    public DbSet<Reservation> Reservations { get; set; }
}