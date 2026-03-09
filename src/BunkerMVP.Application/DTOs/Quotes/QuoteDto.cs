namespace BunkerMVP.Application.DTOs.Quotes;

public class SupplierQuoteDto
{
    public int Id { get; set; }
    public int BunkerRequestId { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime ValidUntil { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateQuoteDto
{
    public int SupplierId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ValidUntil { get; set; }
    public string Notes { get; set; } = string.Empty;
}
