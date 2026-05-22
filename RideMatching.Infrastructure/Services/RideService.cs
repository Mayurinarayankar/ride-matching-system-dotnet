using Microsoft.EntityFrameworkCore;
using RideMatching.Application.DTOs;
using RideMatching.Application.Interfaces;
using RideMatching.Domain.Entities;
using RideMatching.Domain.Enums;
using RideMatching.Infrastructure.Data;

namespace RideMatching.Infrastructure.Services;

public class RideService : IRideService
{
    private readonly AppDbContext _context;

    public RideService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateRideRequestAsync(CreateRideRequestDto dto, Guid userId)
    {
        
        var rider = await _context.Riders
            .FirstOrDefaultAsync(r => r.UserId == userId);

        if (rider == null)
            throw new Exception("Rider profile not found.");

        var ride = new RideRequest
        {
            Id = Guid.NewGuid(),
            RiderId = rider.Id,   
            PickupLatitude = dto.PickupLatitude,
            PickupLongitude = dto.PickupLongitude,
            Status = RideStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        var drivers = await _context.Drivers
            .Where(x => x.IsOnline && x.IsAvailable)
            .ToListAsync();

        var nearest = drivers
            .OrderBy(x =>
                DistanceHelper.Calculate(
                    x.Latitude,
                    x.Longitude,
                    dto.PickupLatitude,
                    dto.PickupLongitude))
            .FirstOrDefault();

        if (nearest != null)
        {
            nearest.IsAvailable = false;
            ride.AssignedDriverId = nearest.Id;
            ride.Status = RideStatus.Matched;
        }

        _context.RideRequests.Add(ride);
        await _context.SaveChangesAsync();

        return ride.Id;
    }

    public async Task<object> GetRideAsync(Guid id)
    {
        var ride = await _context.RideRequests
            .Include(x => x.AssignedDriver)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (ride == null)
            throw new Exception("Ride not found.");

        return ride;
    }
}