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

            builder.Property(p => p.CanalPedido).IsRequired();
            builder.Property(p => p.Status).IsRequired();
            builder.Property(p => p.Total).HasPrecision(10, 2);
            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.LastUpdatedAt);
        }
    }
}
