using BunkerMVP.Application.DTOs.Suppliers;
using BunkerMVP.Domain.Entities;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class SupplierService
{
    private readonly BunkerDbContext _db;

    public SupplierService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<List<SupplierDto>> GetAllAsync()
    {
        return await _db.Suppliers
            .OrderBy(s => s.Name)
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var s = await _db.Suppliers.FindAsync(id);
        return s == null ? null : ToDto(s);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            Country = dto.Country,
            CreatedAt = DateTime.UtcNow
        };
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
        return ToDto(supplier);
    }

    public async Task<SupplierDto?> UpdateAsync(int id, CreateSupplierDto dto)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return null;
        supplier.Name = dto.Name;
        supplier.ContactEmail = dto.ContactEmail;
        supplier.ContactPhone = dto.ContactPhone;
        supplier.Country = dto.Country;
        await _db.SaveChangesAsync();
        return ToDto(supplier);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return false;
        _db.Suppliers.Remove(supplier);
        await _db.SaveChangesAsync();
        return true;
    }

    private static SupplierDto ToDto(Supplier s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        ContactEmail = s.ContactEmail,
        ContactPhone = s.ContactPhone,
        Country = s.Country,
        CreatedAt = s.CreatedAt
    };
}
