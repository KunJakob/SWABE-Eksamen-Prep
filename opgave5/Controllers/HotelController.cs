using Hotels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotels.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> _logger;
    private readonly HotelDbContext _context;

    public HotelController(ILogger<HotelController> logger, HotelDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public async Task<ActionResult<List<Hotel>>> OnGetAsync()
    {
        return await _context.Hotels
                             .Include(h => h.Reviews)
                             .ToListAsync();
    }

    [HttpGet("paginated")]
    public async Task<ActionResult<List<Hotel>>> OnGetAsync([FromQuery] int page, [FromQuery] int pageSize)
    {
        return await _context.Hotels
            .Include(r => r.Reviews)
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    [HttpGet("most_visited")]
    public async Task<ActionResult<List<Hotel>>> OnGetHotelsWithReviewsAsync(int count)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        List<Hotel> allHotelsWithReviews = await _context.Hotels
                                                         .Include(h => h.Reviews)
                                                         .ToListAsync();

        List<Hotel> hotelsWithAtCountReviews = new List<Hotel>();

        foreach (var hotel in allHotelsWithReviews)
        {
            if (hotel.Reviews.Count == count)
                hotelsWithAtCountReviews.Add(hotel);
        }

        sw.Stop();
        var foreachTime = sw.ElapsedMilliseconds;

        sw.Restart();

        var hotels = await _context.Hotels
            .AsNoTracking()
            .Where(h => h.Reviews.Count == count)
            .ToListAsync();

        sw.Stop();

        Console.WriteLine("Foreach loop each hotel took: " + foreachTime.ToString() + " ms");
        Console.WriteLine("Database lookup took: " + sw.ElapsedMilliseconds.ToString() + " ms");

        return hotels;
    }
}
