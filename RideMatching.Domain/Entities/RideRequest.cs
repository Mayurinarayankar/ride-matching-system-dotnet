using RideMatching.Domain.Enums;

namespace RideMatching.Domain.Entities;

public class RideRequest
{
    public Guid Id { get; set; }

    public Guid RiderId { get; set; }

    public Rider Rider { get; set; } = null!;

    public double PickupLatitude { get; set; }

    public double PickupLongitude { get; set; }

    public RideStatus Status { get; set; }

    public Guid? AssignedDriverId { get; set; }

    public Driver? AssignedDriver { get; set; }

    public DateTime RequestedAt { get; set; }
}