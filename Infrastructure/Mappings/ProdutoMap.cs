using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable(nameof(Produto));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.Descricao);
            builder.Property(p => p.Preco).HasPrecision(10, 2);
            builder.Property(p => p.Categoria).IsRequired();
            builder.Property(p => p.Ativo).HasDefaultValue(true);
            builder.Property(p => p.Sazonal).HasDefaultValue(false);
            builder.Property(p => p.CreatedAt);
            builder.Property(p => p.LastUpdatedAt);
        }
    }
}
