namespace BunkerMVP.Application.DTOs.Requests;

public class BunkerRequestDto
{
    public int Id { get; set; }
    public int VesselId { get; set; }
    public string VesselName { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductUnit { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public DateTime ETA { get; set; }
    public string Status { get; set; } = string.Empty;
    public int QuoteCount { get; set; }
    public bool HasOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBunkerRequestDto
{
    public int VesselId { get; set; }
    public int ProductId { get; set; }
    public int LocationId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime ETA { get; set; }
}

public class UpdateBunkerRequestDto
{
    public int VesselId { get; set; }
    public int ProductId { get; set; }
    public int LocationId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime ETA { get; set; }
    public string Status { get; set; } = string.Empty;
}
