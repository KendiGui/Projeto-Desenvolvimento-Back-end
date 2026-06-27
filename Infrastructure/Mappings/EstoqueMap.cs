using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class EstoqueMap : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable(nameof(Estoque));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(e => e.QuantidadeAtual).HasDefaultValue(0);
            builder.Property(e => e.QuantidadeMinima).HasDefaultValue(0);
            builder.Property(e => e.CreatedAt);
            builder.Property(e => e.LastUpdatedAt);

            builder.HasIndex(e => new { e.UnidadeId, e.ProdutoId }).IsUnique();
        }
    }
}
