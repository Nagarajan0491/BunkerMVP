using BunkerMVP.Domain.Enums;

namespace BunkerMVP.Domain.Entities;

public class BunkerOrder
{
    public int Id { get; set; }
    public int BunkerRequestId { get; set; }
    public int SupplierQuoteId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public OrderStatus Status { get; set; } = OrderStatus.Confirmed;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public BunkerRequest BunkerRequest { get; set; } = null!;
    public SupplierQuote SupplierQuote { get; set; } = null!;
}
