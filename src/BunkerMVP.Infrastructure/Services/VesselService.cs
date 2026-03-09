using BunkerMVP.Application.DTOs.Vessels;
using BunkerMVP.Domain.Entities;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class VesselService
{
    private readonly BunkerDbContext _db;

    public VesselService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<List<VesselDto>> GetAllAsync()
    {
        return await _db.Vessels
            .OrderBy(v => v.Name)
            .Select(v => ToDto(v))
            .ToListAsync();
    }

    public async Task<VesselDto?> GetByIdAsync(int id)
    {
        var v = await _db.Vessels.FindAsync(id);
        return v == null ? null : ToDto(v);
    }

    public async Task<VesselDto> CreateAsync(CreateVesselDto dto)
    {
        var vessel = new Vessel
        {
            Name = dto.Name,
            IMONumber = dto.IMONumber,
            VesselType = dto.VesselType,
            Flag = dto.Flag,
            CreatedAt = DateTime.UtcNow
        };
        _db.Vessels.Add(vessel);
        await _db.SaveChangesAsync();
        return ToDto(vessel);
    }

    public async Task<VesselDto?> UpdateAsync(int id, CreateVesselDto dto)
    {
        var vessel = await _db.Vessels.FindAsync(id);
        if (vessel == null) return null;
        vessel.Name = dto.Name;
        vessel.IMONumber = dto.IMONumber;
        vessel.VesselType = dto.VesselType;
        vessel.Flag = dto.Flag;
        await _db.SaveChangesAsync();
        return ToDto(vessel);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vessel = await _db.Vessels.FindAsync(id);
        if (vessel == null) return false;
        _db.Vessels.Remove(vessel);
        await _db.SaveChangesAsync();
        return true;
    }

    private static VesselDto ToDto(Vessel v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        IMONumber = v.IMONumber,
        VesselType = v.VesselType,
        Flag = v.Flag,
        CreatedAt = v.CreatedAt
    };
}
