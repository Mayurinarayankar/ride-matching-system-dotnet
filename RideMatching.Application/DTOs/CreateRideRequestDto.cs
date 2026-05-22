namespace RideMatching.Application.DTOs;

public class CreateRideRequestDto
{
    public Guid RiderId { get; set; }

    public double PickupLatitude { get; set; }

    public double PickupLongitude { get; set; }
}