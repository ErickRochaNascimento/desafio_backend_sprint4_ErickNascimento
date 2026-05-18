using LocadoraAPI.DTOs.Usuario;

namespace LocadoraAPI.Services.Interfaces;

public interface IUsuarioService
{
    Task<List<UsuarioResponseDto>> ListarAsync();
    Task<UsuarioResponseDto?> BuscarPorIdAsync(int id);
    Task<UsuarioResponseDto> CriarAsync(CreateUsuarioDto dto);
    Task<UsuarioResponseDto?> AtualizarAsync(int id, UpdateUsuarioDto dto);
    Task<bool> ExcluirAsync(int id);
}