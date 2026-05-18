namespace LocadoraAPI.DTOs.Usuario;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public bool PrimeiroAcesso { get; set; }
    public DateTime CriadoEm { get; set; }
}