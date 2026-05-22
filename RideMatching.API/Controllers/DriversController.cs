using Microsoft.AspNetCore.Mvc;
using RideMatching.Application.DTOs;
using RideMatching.Application.Interfaces;

namespace RideMatching.API.Controllers;

[ApiController]
[Route("api/drivers")]
public class DriversController : ControllerBase
{
    private readonly IDriverService _service;

    public DriversController(IDriverService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDriverDto dto)
        => Ok(await _service.CreateDriverAsync(dto));

    [HttpPost("{id}/go-online")]
    public async Task<IActionResult> Online(Guid id)
    {
        await _service.GoOnlineAsync(id);
        return Ok();
    }

    [HttpPost("{id}/location")]
    public async Task<IActionResult> Location(Guid id, UpdateLocationDto dto)
    {
        await _service.UpdateLocationAsync(id, dto);
        return Ok();
    }
}