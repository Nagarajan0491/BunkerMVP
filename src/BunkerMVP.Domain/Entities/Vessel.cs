namespace BunkerMVP.Domain.Entities;

public class Vessel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public string VesselType { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
