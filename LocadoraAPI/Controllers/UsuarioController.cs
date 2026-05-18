using LocadoraAPI.DTOs.Usuario;
using LocadoraAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraAPI.Controllers;

[ApiController]
[Route("usuarios")]
[Authorize(Roles = "Administrador")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarios = await _usuarioService.ListarAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var usuario = await _usuarioService.BuscarPorIdAsync(id);

        if (usuario is null)
            return NotFound(new { mensagem = "Funcionário não encontrado." });

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CreateUsuarioDto dto)
    {
        var cpfLimpo = dto.Cpf.Replace(".", "").Replace("-", "");

        if (cpfLimpo.Length != 11)
            return BadRequest(new { mensagem = "CPF deve conter 11 dígitos." });

        var usuario = await _usuarioService.CriarAsync(dto);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = usuario.Id },
            new
            {
                usuario,
                senhaInicial = cpfLimpo[..6],
                aviso = "Informe a senha inicial ao funcionário. Ela deverá ser trocada no primeiro acesso."
            });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] UpdateUsuarioDto dto)
    {
        var usuario = await _usuarioService.AtualizarAsync(id, dto);

        if (usuario is null)
            return NotFound(new { mensagem = "Funcionário não encontrado." });

        return Ok(usuario);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var sucesso = await _usuarioService.ExcluirAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Funcionário não encontrado." });

        return NoContent();
    }
}