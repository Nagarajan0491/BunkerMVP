namespace BunkerMVP.Domain.Entities;

public class SupplierQuote
{
    public int Id { get; set; }
    public int BunkerRequestId { get; set; }
    public int SupplierId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ValidUntil { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public BunkerRequest BunkerRequest { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
    public BunkerOrder? BunkerOrder { get; set; }
}
