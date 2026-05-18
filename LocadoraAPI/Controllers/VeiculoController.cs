using LocadoraAPI.DTOs.Veiculo;
using LocadoraAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraAPI.Controllers;

[ApiController]
[Route("veiculos")]
[Authorize]
public class VeiculoController : ControllerBase
{
    private readonly IVeiculoService _veiculoService;

    public VeiculoController(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? status)
    {
        var veiculos = await _veiculoService.ListarAsync(status);
        return Ok(veiculos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var veiculo = await _veiculoService.BuscarPorIdAsync(id);

        if (veiculo is null)
            return NotFound(new { mensagem = "Veículo não encontrado." });

        return Ok(veiculo);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CreateVeiculoDto dto)
    {
        var veiculo = await _veiculoService.CriarAsync(dto);

        return CreatedAtAction(nameof(BuscarPorId),
            new { id = veiculo.Id }, veiculo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateVeiculoDto dto)
    {
        var veiculo = await _veiculoService.AtualizarAsync(id, dto);

        if (veiculo is null)
            return NotFound(new { mensagem = "Veículo não encontrado." });

        return Ok(veiculo);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            var sucesso = await _veiculoService.ExcluirAsync(id);

            if (!sucesso)
                return NotFound(new { mensagem = "Veículo não encontrado." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}