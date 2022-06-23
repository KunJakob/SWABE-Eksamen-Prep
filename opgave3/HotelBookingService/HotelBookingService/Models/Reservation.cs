using System;
using System.ComponentModel.DataAnnotations;

namespace Consumer.Models.Dto;

public class Reservation
{
    [Key]
    public Guid orderId { get; set; }
    public int hotelId { get; set; }
    public DateTimeOffset checkIn { get; set; }
    public DateTimeOffset checkOut { get; set; }
    public int roomNo { get; set; }
    public string customerName { get; set; }
    public string customerEmail { get; set; }
    public string customerAddress { get; set; }
    
    public Reservation(
        Guid orderId,
        int hotelId, 
        DateTimeOffset checkIn, 
        DateTimeOffset checkOut, 
        int roomNo, 
        string customerName, 
        string customerEmail, 
        string customerAddress)
    {
        this.orderId = orderId;
        this.hotelId = hotelId;
        this.checkIn = checkIn;
        this.checkOut = checkOut;
        this.roomNo = roomNo;
        this.customerName = customerName;
        this.customerEmail = customerEmail;
        this.customerAddress = customerAddress;
    }
    
    public static Reservation MapDtoToReservation(ReservationRequest request)
    {
        return new Reservation(
            Guid.NewGuid(),
            request.hotelId,
            request.checkIn,
            request.checkOut,
            request.roomNo,
            request.customerName,
            request.customerEmail,
            request.customerAddress
        );
    }
}