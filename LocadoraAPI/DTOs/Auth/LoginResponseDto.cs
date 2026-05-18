namespace LocadoraAPI.DTOs.Auth;

public class LoginResponseDto
{
	public string Token { get; set; } = string.Empty;
	public string Nome { get; set; } = string.Empty;
	public string Perfil { get; set; } = string.Empty;
	public bool PrimeiroAcesso { get; set; }
}