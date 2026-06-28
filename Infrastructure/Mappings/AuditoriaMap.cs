using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class AuditoriaMap : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            builder.ToTable(nameof(Auditoria));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(a => a.Acao).IsRequired();
            builder.Property(a => a.Entidade).IsRequired();
            builder.Property(a => a.EntidadeId);
            builder.Property(a => a.DadosAntes);
            builder.Property(a => a.DadosDepois);
            builder.Property(a => a.Ip);
            builder.Property(a => a.CreatedAt);
            builder.Property(a => a.LastUpdatedAt);

            builder.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
