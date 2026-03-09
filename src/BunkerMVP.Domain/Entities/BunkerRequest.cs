using BunkerMVP.Domain.Enums;

namespace BunkerMVP.Domain.Entities;

public class BunkerRequest
{
    public int Id { get; set; }
    public int VesselId { get; set; }
    public int ProductId { get; set; }
    public int LocationId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime ETA { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Draft;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Vessel Vessel { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public Location Location { get; set; } = null!;
    public ICollection<SupplierQuote> SupplierQuotes { get; set; } = new List<SupplierQuote>();
    public BunkerOrder? BunkerOrder { get; set; }
}
