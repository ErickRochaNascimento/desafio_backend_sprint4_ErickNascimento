using LocadoraAPI.Data;
using LocadoraAPI.DTOs.Veiculo;
using LocadoraAPI.Models;
using LocadoraAPI.Models.Enums;
using LocadoraAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocadoraAPI.Services;

public class VeiculoService : IVeiculoService
{
    private readonly AppDbContext _context;

    public VeiculoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<VeiculoResponseDto>> ListarAsync(string? status)
    {
        var query = _context.Veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(status) &&
            Enum.TryParse<StatusVeiculo>(status, true, out var statusEnum))
        {
            query = query.Where(v => v.Status == statusEnum);
        }

        return await query
            .Select(v => ToDto(v))
            .ToListAsync();
    }

    public async Task<VeiculoResponseDto?> BuscarPorIdAsync(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        return veiculo is null ? null : ToDto(veiculo);
    }

    public async Task<VeiculoResponseDto> CriarAsync(CreateVeiculoDto dto)
    {
        var veiculo = new Veiculo
        {
            Placa = dto.Placa.ToUpper().Replace("-", ""),
            Modelo = dto.Modelo,
            Marca = dto.Marca,
            Ano = dto.Ano,
            Cor = dto.Cor,
            Status = StatusVeiculo.Disponivel
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();
        return ToDto(veiculo);
    }

    public async Task<VeiculoResponseDto?> AtualizarAsync(int id, UpdateVeiculoDto dto)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo is null) return null;

        veiculo.Modelo = dto.Modelo;
        veiculo.Marca = dto.Marca;
        veiculo.Ano = dto.Ano;
        veiculo.Cor = dto.Cor;

        await _context.SaveChangesAsync();
        return ToDto(veiculo);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo is null) return false;

        if (veiculo.Status == StatusVeiculo.Alugado)
            throw new InvalidOperationException("Não é possível excluir um veículo alugado.");

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();
        return true;
    }

    private static VeiculoResponseDto ToDto(Veiculo v) => new()
    {
        Id = v.Id,
        Placa = v.Placa,
        Modelo = v.Modelo,
        Marca = v.Marca,
        Ano = v.Ano,
        Cor = v.Cor,
        Status = v.Status.ToString(),
        CriadoEm = v.CriadoEm
    };
}