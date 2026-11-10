using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanesController : ControllerBase
{
    private readonly IServices _services;

    public PlanesController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlanes()
    {
        try
        {
            return Ok(await _services.GetPlanesAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
