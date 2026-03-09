using BunkerMVP.Domain.Enums;

namespace BunkerMVP.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProductUnit Unit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
