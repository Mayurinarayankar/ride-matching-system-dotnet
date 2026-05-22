using RideMatching.Application.DTOs;

namespace RideMatching.Application.Interfaces;

public interface IRideService
{
    Task<Guid> CreateRideRequestAsync(CreateRideRequestDto dto, Guid userId);

    Task<object> GetRideAsync(Guid id);
}