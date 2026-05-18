using LocadoraAPI.Models;
using LocadoraAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LocadoraAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Locacao> Locacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            e.Property(u => u.Cpf).IsRequired().HasMaxLength(11);
            e.HasIndex(u => u.Cpf).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(100);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.SenhaHash).IsRequired();
            e.Property(u => u.Perfil).HasConversion<string>();
        });

        modelBuilder.Entity<Veiculo>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.Placa).IsRequired().HasMaxLength(8);
            e.HasIndex(v => v.Placa).IsUnique();
            e.Property(v => v.Modelo).IsRequired().HasMaxLength(100);
            e.Property(v => v.Marca).IsRequired().HasMaxLength(50);
            e.Property(v => v.Cor).IsRequired().HasMaxLength(30);
            e.Property(v => v.Status).HasConversion<string>();
        });

        modelBuilder.Entity<Locacao>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.NomeCliente).IsRequired().HasMaxLength(100);
            e.Property(l => l.CpfCliente).IsRequired().HasMaxLength(11);
            e.Property(l => l.Status).HasConversion<string>();

            e.HasOne(l => l.Veiculo)
             .WithMany(v => v.Locacoes)
             .HasForeignKey(l => l.VeiculoId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(l => l.Usuario)
             .WithMany(u => u.Locacoes)
             .HasForeignKey(l => l.UsuarioId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            Nome = "Administrador",
            Cpf = "00000000000",
            Email = "admin@locadora.com",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Perfil = PerfilUsuario.Administrador,
            PrimeiroAcesso = false,
            CriadoEm = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}