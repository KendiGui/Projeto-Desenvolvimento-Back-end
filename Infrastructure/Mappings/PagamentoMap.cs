using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable(nameof(Pagamento));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Ignore(x => x.guid);

            builder.Property(pag => pag.PedidoId).IsRequired();
            builder.Property(pag => pag.Status).IsRequired();
            builder.Property(pag => pag.FormaPagamento).IsRequired();
            builder.Property(pag => pag.GatewayTransactionId);
            builder.Property(pag => pag.PayloadRequest).HasColumnType("TEXT");
            builder.Property(pag => pag.PayloadResponse).HasColumnType("TEXT");
            builder.Property(pag => pag.CreatedAt);
            builder.Property(pag => pag.LastUpdatedAt);

            builder.HasOne<Pedido>()
                   .WithOne(p => p.Pagamento)
                   .HasForeignKey<Pagamento>(pag => pag.PedidoId);
        }
    }
}
