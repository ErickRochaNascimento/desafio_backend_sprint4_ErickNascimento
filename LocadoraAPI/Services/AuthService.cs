using LocadoraAPI.Data;
using LocadoraAPI.DTOs.Auth;
using LocadoraAPI.Helpers;
using LocadoraAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocadoraAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(AppDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            return null;

        var token = _jwtHelper.GerarToken(
            usuario.Id,
            usuario.Nome,
            usuario.Perfil.ToString(),
            usuario.PrimeiroAcesso);

        return new LoginResponseDto
        {
            Token = token,
            Nome = usuario.Nome,
            Perfil = usuario.Perfil.ToString(),
            PrimeiroAcesso = usuario.PrimeiroAcesso
        };
    }

    public async Task<bool> AlterarSenhaAsync(int usuarioId, AlterarSenhaDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);

        if (usuario is null)
            return false;

        if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, usuario.SenhaHash))
            return false;

        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
        usuario.PrimeiroAcesso = false;

        await _context.SaveChangesAsync();
        return true;
    }
}