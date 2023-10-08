using Microsoft.EntityFrameworkCore;

namespace PDV_Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar o relacionamento entre Produto e Cliente
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<Produto>()
                .Property(p => p.Origem)
                .HasConversion<string>();

            modelBuilder.Entity<Produto>()
                .HasKey(p => p.Referencia);
        }
    }
}
