using ExternalService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ExternalService.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelBookingController : ControllerBase
{
    private readonly ILogger<HotelBookingController> _logger;
    private readonly RabbitClient _rabbitClient;

    public HotelBookingController(ILogger<HotelBookingController> logger,
        RabbitClient rabbitClient)
    {
        _logger = logger;
        _rabbitClient = rabbitClient;
    }

    [HttpPost(Name = "BookHotel")]
    public IActionResult Post([FromBody] ReservationRequest request)
    {
        _logger.LogInformation("Booking hotel");
        _rabbitClient.SendRequest(request);
        return Ok();
    }
}