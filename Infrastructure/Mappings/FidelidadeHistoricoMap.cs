using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class FidelidadeHistoricoMap : IEntityTypeConfiguration<FidelidadeHistorico>
    {
        public void Configure(EntityTypeBuilder<FidelidadeHistorico> builder)
        {
            builder.ToTable(nameof(FidelidadeHistorico));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(h => h.Tipo).IsRequired();
            builder.Property(h => h.Pontos).IsRequired();
            builder.Property(h => h.Descricao);
            builder.Property(h => h.CreatedAt);
            builder.Property(h => h.LastUpdatedAt);

            builder.HasOne(h => h.Cliente)
                .WithMany()
                .HasForeignKey(h => h.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(h => h.Pedido)
                .WithMany()
                .HasForeignKey(h => h.PedidoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
