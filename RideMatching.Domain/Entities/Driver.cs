using System.ComponentModel.DataAnnotations;

namespace RideMatching.Domain.Entities;

public class Driver
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public bool IsOnline { get; set; }

    public bool IsAvailable { get; set; } = true;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}