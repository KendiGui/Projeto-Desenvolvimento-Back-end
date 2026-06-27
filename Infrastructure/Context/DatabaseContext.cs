using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Cadastros base
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<UnidadeProduto> UnidadeProdutos { get; set; }

        // Estoque
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<MovimentoEstoque> MovimentosEstoque { get; set; }

        // Pedidos e Pagamentos
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        // Autenticação
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warn => warn.Ignore(CoreEventId.DetachedLazyLoadingWarning));
            optionsBuilder.EnableDetailedErrors();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
