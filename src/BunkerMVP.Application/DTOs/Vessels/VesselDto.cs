namespace BunkerMVP.Application.DTOs.Vessels;

public class VesselDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public string VesselType { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateVesselDto
{
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public string VesselType { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
}
