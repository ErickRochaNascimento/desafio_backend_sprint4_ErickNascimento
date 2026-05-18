namespace LocadoraAPI.DTOs.Veiculo;

public class UpdateVeiculoDto
{
	public string Modelo { get; set; } = string.Empty;
	public string Marca { get; set; } = string.Empty;
	public int Ano { get; set; }
	public string Cor { get; set; } = string.Empty;
}