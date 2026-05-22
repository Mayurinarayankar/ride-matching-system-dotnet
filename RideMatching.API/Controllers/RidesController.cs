using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMatching.Application.DTOs;
using RideMatching.Application.Interfaces;

namespace RideMatching.API.Controllers;

[ApiController]
[Route("api/rides")]
[Authorize(Roles = "Rider,Driver")]
public class RidesController : ControllerBase
{
    private readonly IRideService _service;

    public RidesController(IRideService service)
    {
        _service = service;
    }

    [HttpPost("request")]
    public async Task<IActionResult> Request(CreateRideRequestDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        return Ok(await _service.CreateRideRequestAsync(dto, userId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _service.GetRideAsync(id));
}