namespace BunkerMVP.Application.DTOs.Orders;

public class BunkerOrderDto
{
    public int Id { get; set; }
    public int BunkerRequestId { get; set; }
    public int SupplierQuoteId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateOrderDto
{
    public int SupplierQuoteId { get; set; }
    public string Notes { get; set; } = string.Empty;
}
