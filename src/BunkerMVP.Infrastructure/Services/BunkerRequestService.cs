using BunkerMVP.Application.DTOs.Orders;
using BunkerMVP.Application.DTOs.Quotes;
using BunkerMVP.Application.DTOs.Requests;
using BunkerMVP.Domain.Entities;
using BunkerMVP.Domain.Enums;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class BunkerRequestService
{
    private readonly BunkerDbContext _db;

    public BunkerRequestService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<List<BunkerRequestDto>> GetAllAsync()
    {
        return await _db.BunkerRequests
            .Include(r => r.Vessel)
            .Include(r => r.Product)
            .Include(r => r.Location)
            .Include(r => r.SupplierQuotes)
            .Include(r => r.BunkerOrder)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => ToDto(r))
            .ToListAsync();
    }

    public async Task<BunkerRequestDto?> GetByIdAsync(int id)
    {
        var r = await _db.BunkerRequests
            .Include(r => r.Vessel)
            .Include(r => r.Product)
            .Include(r => r.Location)
            .Include(r => r.SupplierQuotes)
            .Include(r => r.BunkerOrder)
            .FirstOrDefaultAsync(r => r.Id == id);
        return r == null ? null : ToDto(r);
    }

    public async Task<BunkerRequestDto> CreateAsync(CreateBunkerRequestDto dto)
    {
        var request = new BunkerRequest
        {
            VesselId = dto.VesselId,
            ProductId = dto.ProductId,
            LocationId = dto.LocationId,
            Quantity = dto.Quantity,
            ETA = dto.ETA.ToUniversalTime(),
            Status = RequestStatus.Open,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _db.BunkerRequests.Add(request);
        await _db.SaveChangesAsync();

        await _db.Entry(request).Reference(r => r.Vessel).LoadAsync();
        await _db.Entry(request).Reference(r => r.Product).LoadAsync();
        await _db.Entry(request).Reference(r => r.Location).LoadAsync();

        return ToDto(request);
    }

    public async Task<BunkerRequestDto?> UpdateAsync(int id, UpdateBunkerRequestDto dto)
    {
        var request = await _db.BunkerRequests
            .Include(r => r.Vessel)
            .Include(r => r.Product)
            .Include(r => r.Location)
            .Include(r => r.SupplierQuotes)
            .Include(r => r.BunkerOrder)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (request == null) return null;

        request.VesselId = dto.VesselId;
        request.ProductId = dto.ProductId;
        request.LocationId = dto.LocationId;
        request.Quantity = dto.Quantity;
        request.ETA = dto.ETA.ToUniversalTime();
        if (Enum.TryParse<RequestStatus>(dto.Status, out var status))
            request.Status = status;
        request.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        await _db.Entry(request).Reference(r => r.Vessel).LoadAsync();
        await _db.Entry(request).Reference(r => r.Product).LoadAsync();
        await _db.Entry(request).Reference(r => r.Location).LoadAsync();

        return ToDto(request);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var request = await _db.BunkerRequests.FindAsync(id);
        if (request == null) return false;
        _db.BunkerRequests.Remove(request);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<SupplierQuoteDto>> GetQuotesAsync(int requestId)
    {
        return await _db.SupplierQuotes
            .Include(q => q.Supplier)
            .Where(q => q.BunkerRequestId == requestId)
            .OrderBy(q => q.Price)
            .Select(q => ToQuoteDto(q))
            .ToListAsync();
    }

    public async Task<SupplierQuoteDto> AddQuoteAsync(int requestId, CreateQuoteDto dto)
    {
        var quote = new SupplierQuote
        {
            BunkerRequestId = requestId,
            SupplierId = dto.SupplierId,
            Price = dto.Price,
            Currency = dto.Currency,
            ValidUntil = dto.ValidUntil.ToUniversalTime(),
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        _db.SupplierQuotes.Add(quote);

        var request = await _db.BunkerRequests.FindAsync(requestId);
        if (request != null && request.Status == RequestStatus.Open)
        {
            request.Status = RequestStatus.Quoted;
            request.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        await _db.Entry(quote).Reference(q => q.Supplier).LoadAsync();
        return ToQuoteDto(quote);
    }

    public async Task<BunkerOrderDto> CreateOrderAsync(int requestId, CreateOrderDto dto)
    {
        var quote = await _db.SupplierQuotes.FindAsync(dto.SupplierQuoteId)
            ?? throw new InvalidOperationException("Quote not found");

        var request = await _db.BunkerRequests.FindAsync(requestId)
            ?? throw new InvalidOperationException("Request not found");

        var totalAmount = quote.Price * request.Quantity;

        var order = new BunkerOrder
        {
            BunkerRequestId = requestId,
            SupplierQuoteId = dto.SupplierQuoteId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Currency = quote.Currency,
            Status = OrderStatus.Confirmed,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        _db.BunkerOrders.Add(order);

        request.Status = RequestStatus.Ordered;
        request.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return ToOrderDto(order);
    }

    private static BunkerRequestDto ToDto(BunkerRequest r) => new()
    {
        Id = r.Id,
        VesselId = r.VesselId,
        VesselName = r.Vessel?.Name ?? string.Empty,
        ProductId = r.ProductId,
        ProductName = r.Product?.Name ?? string.Empty,
        ProductUnit = r.Product?.Unit.ToString() ?? string.Empty,
        LocationId = r.LocationId,
        LocationName = r.Location?.Name ?? string.Empty,
        Quantity = r.Quantity,
        ETA = r.ETA,
        Status = r.Status.ToString(),
        QuoteCount = r.SupplierQuotes?.Count ?? 0,
        HasOrder = r.BunkerOrder != null,
        CreatedAt = r.CreatedAt,
        UpdatedAt = r.UpdatedAt
    };

    private static SupplierQuoteDto ToQuoteDto(SupplierQuote q) => new()
    {
        Id = q.Id,
        BunkerRequestId = q.BunkerRequestId,
        SupplierId = q.SupplierId,
        SupplierName = q.Supplier?.Name ?? string.Empty,
        Price = q.Price,
        Currency = q.Currency,
        ValidUntil = q.ValidUntil,
        Notes = q.Notes,
        CreatedAt = q.CreatedAt
    };

    private static BunkerOrderDto ToOrderDto(BunkerOrder o) => new()
    {
        Id = o.Id,
        BunkerRequestId = o.BunkerRequestId,
        SupplierQuoteId = o.SupplierQuoteId,
        OrderDate = o.OrderDate,
        TotalAmount = o.TotalAmount,
        Currency = o.Currency,
        Status = o.Status.ToString(),
        Notes = o.Notes,
        CreatedAt = o.CreatedAt
    };
}
