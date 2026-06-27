using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class MovimentoEstoqueMap : IEntityTypeConfiguration<MovimentoEstoque>
    {
        public void Configure(EntityTypeBuilder<MovimentoEstoque> builder)
        {
            builder.ToTable(nameof(MovimentoEstoque));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Ignore(x => x.guid);

            builder.Property(me => me.EstoqueId).IsRequired();
            builder.Property(me => me.TipoMovimento).IsRequired();
            builder.Property(me => me.Quantidade).IsRequired();
            builder.Property(me => me.Motivo);
            builder.Property(me => me.UsuarioId);
            builder.Property(me => me.CreatedAt);
            builder.Property(me => me.LastUpdatedAt);

            builder.HasOne<Estoque>()
                   .WithMany(e => e.Movimentos)
                   .HasForeignKey(me => me.EstoqueId);

            builder.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey(me => me.UsuarioId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
