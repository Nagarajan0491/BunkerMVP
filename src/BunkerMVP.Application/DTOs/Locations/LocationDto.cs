namespace BunkerMVP.Application.DTOs.Locations;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateLocationDto
{
    public string Name { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
