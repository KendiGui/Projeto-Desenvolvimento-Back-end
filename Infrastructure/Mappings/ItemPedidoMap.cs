using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class ItemPedidoMap : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable(nameof(ItemPedido));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Ignore(x => x.guid);

            builder.Property(ip => ip.PedidoId).IsRequired();
            builder.Property(ip => ip.ProdutoId).IsRequired();
            builder.Property(ip => ip.Quantidade).IsRequired();
            builder.Property(ip => ip.PrecoUnitario).HasPrecision(10, 2);
            builder.Property(ip => ip.Subtotal).HasPrecision(10, 2);
            builder.Property(ip => ip.CreatedAt);
            builder.Property(ip => ip.LastUpdatedAt);

            builder.HasOne<Pedido>()
                   .WithMany(p => p.Itens)
                   .HasForeignKey(ip => ip.PedidoId);

            builder.HasOne<Produto>()
                   .WithMany(pr => pr.ItemPedidos)
                   .HasForeignKey(ip => ip.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
