using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class UnidadeMap : IEntityTypeConfiguration<Unidade>
    {
        public void Configure(EntityTypeBuilder<Unidade> builder)
        {
            builder.ToTable(nameof(Unidade));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(u => u.Nome).IsRequired();
            builder.Property(u => u.Cidade).IsRequired();
            builder.Property(u => u.Estado).IsRequired();
            builder.Property(u => u.Endereco).IsRequired();
            builder.Property(u => u.Ativa).HasDefaultValue(true);
            builder.Property(u => u.CreatedAt);
            builder.Property(u => u.LastUpdatedAt);
        }
    }
}
