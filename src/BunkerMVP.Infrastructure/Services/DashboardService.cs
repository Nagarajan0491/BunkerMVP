using BunkerMVP.Application.DTOs.Dashboard;
using BunkerMVP.Domain.Enums;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class DashboardService
{
    private readonly BunkerDbContext _db;

    public DashboardService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardStatsDto> GetStatsAsync()
    {
        var totalRequests = await _db.BunkerRequests.CountAsync();
        var openRequests = await _db.BunkerRequests
            .CountAsync(r => r.Status == RequestStatus.Open || r.Status == RequestStatus.Draft);
        var pendingQuotes = await _db.BunkerRequests
            .CountAsync(r => r.Status == RequestStatus.Quoted);
        var totalOrders = await _db.BunkerOrders.CountAsync();

        return new DashboardStatsDto
        {
            TotalRequests = totalRequests,
            OpenRequests = openRequests,
            PendingQuotes = pendingQuotes,
            TotalOrders = totalOrders
        };
    }
}
