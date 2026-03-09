using BunkerMVP.Application.DTOs.Locations;
using BunkerMVP.Domain.Entities;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class LocationService
{
    private readonly BunkerDbContext _db;

    public LocationService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<List<LocationDto>> GetAllAsync()
    {
        return await _db.Locations
            .OrderBy(l => l.Name)
            .Select(l => ToDto(l))
            .ToListAsync();
    }

    public async Task<LocationDto?> GetByIdAsync(int id)
    {
        var l = await _db.Locations.FindAsync(id);
        return l == null ? null : ToDto(l);
    }

    public async Task<LocationDto> CreateAsync(CreateLocationDto dto)
    {
        var location = new Location
        {
            Name = dto.Name,
            Port = dto.Port,
            Country = dto.Country,
            CreatedAt = DateTime.UtcNow
        };
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();
        return ToDto(location);
    }

    public async Task<LocationDto?> UpdateAsync(int id, CreateLocationDto dto)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location == null) return null;
        location.Name = dto.Name;
        location.Port = dto.Port;
        location.Country = dto.Country;
        await _db.SaveChangesAsync();
        return ToDto(location);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location == null) return false;
        _db.Locations.Remove(location);
        await _db.SaveChangesAsync();
        return true;
    }

    private static LocationDto ToDto(Location l) => new()
    {
        Id = l.Id,
        Name = l.Name,
        Port = l.Port,
        Country = l.Country,
        CreatedAt = l.CreatedAt
    };
}
