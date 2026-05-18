using LocadoraAPI.Models.Enums;

namespace LocadoraAPI.Models;

public class Veiculo
{
	public int Id { get; set; }
	public string Placa { get; set; } = string.Empty;
	public string Modelo { get; set; } = string.Empty;
	public string Marca { get; set; } = string.Empty;
	public int Ano { get; set; }
	public string Cor { get; set; } = string.Empty;
	public StatusVeiculo Status { get; set; } = StatusVeiculo.Disponivel;
	public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

	public ICollection<Locacao> Locacoes { get; set; } = new List<Locacao>();
}