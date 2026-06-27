using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class UnidadeProdutoMap : IEntityTypeConfiguration<UnidadeProduto>
    {
        public void Configure(EntityTypeBuilder<UnidadeProduto> builder)
        {
            builder.ToTable(nameof(UnidadeProduto));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(up => up.Disponivel).HasDefaultValue(true);
            builder.Property(up => up.PrecoCustomizado).HasPrecision(10, 2);
            builder.Property(up => up.CreatedAt);
            builder.Property(up => up.LastUpdatedAt);
        }
    }
}
