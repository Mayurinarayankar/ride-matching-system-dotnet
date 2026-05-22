using RideMatching.Application.DTOs;

namespace RideMatching.Application.Interfaces;

public interface IDriverService
{
    Task<Guid> CreateDriverAsync(CreateDriverDto dto, Guid userId); 
    Task GoOnlineAsync(Guid driverId);

    Task UpdateLocationAsync(Guid driverId, UpdateLocationDto dto);
}