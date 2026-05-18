using LocadoraAPI.DTOs.Veiculo;

namespace LocadoraAPI.Services.Interfaces;

public interface IVeiculoService
{
    Task<List<VeiculoResponseDto>> ListarAsync(string? status);
    Task<VeiculoResponseDto?> BuscarPorIdAsync(int id);
    Task<VeiculoResponseDto> CriarAsync(CreateVeiculoDto dto);
    Task<VeiculoResponseDto?> AtualizarAsync(int id, UpdateVeiculoDto dto);
    Task<bool> ExcluirAsync(int id);
}