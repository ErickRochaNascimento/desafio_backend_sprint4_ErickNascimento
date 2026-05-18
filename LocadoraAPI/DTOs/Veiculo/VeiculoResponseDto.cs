namespace LocadoraAPI.DTOs.Veiculo;

public class VeiculoResponseDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}