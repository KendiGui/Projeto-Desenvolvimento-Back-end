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
            builder.Ignore(x => x.guid);

            builder.Property(e => e.UnidadeId).IsRequired();
            builder.Property(e => e.ProdutoId).IsRequired();
            builder.Property(e => e.QuantidadeAtual).HasDefaultValue(0);
            builder.Property(e => e.QuantidadeMinima).HasDefaultValue(0);
            builder.Property(e => e.CreatedAt);
            builder.Property(e => e.LastUpdatedAt);

            builder.HasOne<Unidade>()
                   .WithMany(u => u.Estoques)
                   .HasForeignKey(e => e.UnidadeId);

            builder.HasOne<Produto>()
                   .WithMany(p => p.Estoques)
                   .HasForeignKey(e => e.ProdutoId);

            builder.HasMany<MovimentoEstoque>()
                   .WithOne(me => me.Estoque)
                   .HasForeignKey(me => me.EstoqueId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.UnidadeId, e.ProdutoId }).IsUnique();
        }
    }
}
