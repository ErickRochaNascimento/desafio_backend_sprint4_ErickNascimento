using LocadoraAPI.Models.Enums;

namespace LocadoraAPI.Models;

public class Locacao
{
    public int Id { get; set; }

    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public string NomeCliente { get; set; } = string.Empty;
    public string CpfCliente { get; set; } = string.Empty;

    public DateTime DataInicio { get; set; }
    public DateTime DataDevolucaoPrevista { get; set; }
    public DateTime? DataDevolucaoReal { get; set; }

    public StatusLocacao Status { get; set; } = StatusLocacao.Pendente;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}