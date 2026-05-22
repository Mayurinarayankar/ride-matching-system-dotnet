using RideMatching.Domain.Entities;

namespace RideMatching.Domain.Entities;

public class Rider
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}