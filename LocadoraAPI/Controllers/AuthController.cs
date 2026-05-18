using System.Security.Claims;
using LocadoraAPI.DTOs.Auth;
using LocadoraAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var resultado = await _authService.LoginAsync(dto);

        if (resultado is null)
            return Unauthorized(new { mensagem = "Email ou senha inválidos." });

        return Ok(resultado);
    }

    [Authorize]
    [HttpPost("alterar-senha")]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto dto)
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (usuarioIdClaim is null)
            return Unauthorized();

        var usuarioId = int.Parse(usuarioIdClaim);
        var sucesso = await _authService.AlterarSenhaAsync(usuarioId, dto);

        if (!sucesso)
            return BadRequest(new { mensagem = "Senha atual incorreta." });

        return Ok(new { mensagem = "Senha alterada com sucesso." });
    }
}