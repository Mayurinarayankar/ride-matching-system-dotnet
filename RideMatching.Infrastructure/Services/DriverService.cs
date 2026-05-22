using RideMatching.Application.DTOs;
using RideMatching.Application.Interfaces;
using RideMatching.Domain.Entities;
using RideMatching.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace RideMatching.Infrastructure.Services;

public class DriverService : IDriverService
{
    private readonly AppDbContext _context;

    public DriverService(AppDbContext context)
    {
        _context = context;
    }

    public async Task GoOnlineAsync(Guid driverId)
    {
        var driver = await _context.Drivers.FindAsync(driverId);

        driver!.IsOnline = true;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateLocationAsync(Guid driverId, UpdateLocationDto dto)
    {
        var driver = await _context.Drivers.FindAsync(driverId);

        driver!.Latitude = dto.Latitude;
        driver.Longitude = dto.Longitude;

        await _context.SaveChangesAsync();
    }

    public async Task<Guid> CreateDriverAsync(CreateDriverDto dto, Guid userId)
    {
        var existing = await _context.Drivers
            .FirstOrDefaultAsync(d => d.UserId == userId);

        if (existing != null)
            throw new Exception("Driver profile already exists.");

        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            UserId = userId,
            IsOnline = false,
            IsAvailable = true
        };

        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        return driver.Id;
    }
}