using LocadoraAPI.Data;
using LocadoraAPI.DTOs.Usuario;
using LocadoraAPI.Models;
using LocadoraAPI.Models.Enums;
using LocadoraAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocadoraAPI.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UsuarioResponseDto>> ListarAsync()
    {
        return await _context.Usuarios
            .Where(u => u.Perfil == PerfilUsuario.Funcionario)
            .Select(u => ToDto(u))
            .ToListAsync();
    }

    public async Task<UsuarioResponseDto?> BuscarPorIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        return usuario is null ? null : ToDto(usuario);
    }

    public async Task<UsuarioResponseDto> CriarAsync(CreateUsuarioDto dto)
    {
        // Senha inicial = 6 primeiros dígitos do CPF
        var senhaInicial = dto.Cpf.Replace(".", "").Replace("-", "")[..6];

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf.Replace(".", "").Replace("-", ""),
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senhaInicial),
            Perfil = PerfilUsuario.Funcionario,
            PrimeiroAcesso = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return ToDto(usuario);
    }

    public async Task<UsuarioResponseDto?> AtualizarAsync(int id, UpdateUsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario is null) return null;

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;

        await _context.SaveChangesAsync();
        return ToDto(usuario);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario is null) return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    private static UsuarioResponseDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        Nome = u.Nome,
        Cpf = u.Cpf,
        Email = u.Email,
        Perfil = u.Perfil.ToString(),
        PrimeiroAcesso = u.PrimeiroAcesso,
        CriadoEm = u.CriadoEm
    };
}