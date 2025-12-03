using Microsoft.AspNetCore.Mvc;
using DinoApiRest.Services;
using DinoApiRest.Dtos;
using DinoApiRest.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DinoApiRest.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DinosController : ControllerBase
{
    private readonly IDinoService _dinoService;

    public DinosController(IDinoService dinoService)
    {
        _dinoService = dinoService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DinoDto>> GetDinoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest(new { Message = "El id no puede ser vacío" });
        try
        {
            var dino = await _dinoService.GetDinoByIdAsync(id, cancellationToken);
            return Ok(dino);
        }
        catch (DinoNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error al ingresar datos", Detalle = ex.Message });
        }
    }
}
