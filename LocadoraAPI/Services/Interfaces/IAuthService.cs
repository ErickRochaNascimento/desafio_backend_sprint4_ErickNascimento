using LocadoraAPI.DTOs.Auth;

namespace LocadoraAPI.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    Task<bool> AlterarSenhaAsync(int usuarioId, AlterarSenhaDto dto);
}