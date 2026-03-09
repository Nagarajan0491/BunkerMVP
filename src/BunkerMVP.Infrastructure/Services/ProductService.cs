using BunkerMVP.Application.DTOs.Products;
using BunkerMVP.Domain.Entities;
using BunkerMVP.Domain.Enums;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class ProductService
{
    private readonly BunkerDbContext _db;

    public ProductService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _db.Products
            .OrderBy(p => p.Name)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await _db.Products.FindAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            ProductCode = dto.ProductCode,
            Description = dto.Description,
            Unit = Enum.Parse<ProductUnit>(dto.Unit),
            CreatedAt = DateTime.UtcNow
        };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return ToDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(int id, CreateProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return null;
        product.Name = dto.Name;
        product.ProductCode = dto.ProductCode;
        product.Description = dto.Description;
        product.Unit = Enum.Parse<ProductUnit>(dto.Unit);
        await _db.SaveChangesAsync();
        return ToDto(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        ProductCode = p.ProductCode,
        Description = p.Description,
        Unit = p.Unit.ToString(),
        CreatedAt = p.CreatedAt
    };
}
