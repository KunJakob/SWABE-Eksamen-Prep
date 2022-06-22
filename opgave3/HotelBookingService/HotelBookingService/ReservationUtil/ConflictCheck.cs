using System;
using System.Linq;
using Consumer.Database;
using Consumer.Models.Dto;

namespace Consumer.ReservationUtil;

public static class ConflictCheck
{
    public static Reservation EnsureNoConflictingReservation(ReservationRequest request, ReservationDbContext db)
    {
        // Ensure no conflicting reservations
        Console.WriteLine(Environment.MachineName + " - " + DateTime.Now.Millisecond +" - Checking for conflicting reservations");
        
        var conflictingReservations = db.Reservations.Where(r => 
            r.hotelId == request.hotelId 
            && r.roomNo == request.roomNo 
            && r.checkIn <= request.checkOut
            && r.checkOut >= request.checkIn).ToList(); 
        
        if (conflictingReservations.Any())
        {
            Console.WriteLine(Environment.MachineName + " - " + DateTime.Now.Millisecond +" - Found conflicting reservations");
            throw new Exception("Conflicting reservation found");
        }
        // otherwise save the reservation
        var reservation = Reservation.MapDtoToReservation(request);

        Console.WriteLine(Environment.MachineName + " - " + DateTime.Now.Millisecond +" - Saving reservation");
        db.Reservations.Add(reservation);
        db.SaveChanges();
        return reservation;
    }
}