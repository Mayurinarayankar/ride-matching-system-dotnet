using RideMatching.Application.DTOs;

namespace RideMatching.Application.Interfaces;

public interface IDriverService
{
    Task<Guid> CreateDriverAsync(CreateDriverDto dto);

    Task GoOnlineAsync(Guid driverId);

    Task UpdateLocationAsync(Guid driverId, UpdateLocationDto dto);
}