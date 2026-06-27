using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable(nameof(Pedido));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Ignore(x => x.guid);

            builder.Property(p => p.ClienteId).IsRequired();
            builder.Property(p => p.UnidadeId).IsRequired();
            builder.Property(p => p.CanalPedido).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Total).HasPrecision(10, 2);
            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.LastUpdatedAt);

            builder.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey(p => p.ClienteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Unidade>()
                   .WithMany(u => u.Pedidos)
                   .HasForeignKey(p => p.UnidadeId);

            builder.HasMany<ItemPedido>()
                   .WithOne(ip => ip.Pedido)
                   .HasForeignKey(ip => ip.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Pagamento>()
                   .WithOne(pag => pag.Pedido)
                   .HasForeignKey<Pagamento>(pag => pag.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
