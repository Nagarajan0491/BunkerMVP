using BunkerMVP.Application.DTOs.Orders;
using BunkerMVP.Application.DTOs.Quotes;
using BunkerMVP.Application.DTOs.Requests;
using BunkerMVP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BunkerMVP.API.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private readonly BunkerRequestService _service;

    public RequestsController(BunkerRequestService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBunkerRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBunkerRequestDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("{id}/quotes")]
    public async Task<IActionResult> GetQuotes(int id)
    {
        var quotes = await _service.GetQuotesAsync(id);
        return Ok(quotes);
    }

    [HttpPost("{id}/quotes")]
    public async Task<IActionResult> AddQuote(int id, [FromBody] CreateQuoteDto dto)
    {
        var result = await _service.AddQuoteAsync(id, dto);
        return Ok(result);
    }

    [HttpPost("{id}/orders")]
    public async Task<IActionResult> CreateOrder(int id, [FromBody] CreateOrderDto dto)
    {
        try
        {
            var result = await _service.CreateOrderAsync(id, dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
