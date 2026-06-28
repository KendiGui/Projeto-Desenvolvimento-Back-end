using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class FidelidadeContaMap : IEntityTypeConfiguration<FidelidadeConta>
    {
        public void Configure(EntityTypeBuilder<FidelidadeConta> builder)
        {
            builder.ToTable(nameof(FidelidadeConta));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(c => c.Pontos).HasDefaultValue(0);
            builder.Property(c => c.ConsentimentoMarketing).HasDefaultValue(false);
            builder.Property(c => c.ConsentimentoData);
            builder.Property(c => c.CreatedAt);
            builder.Property(c => c.LastUpdatedAt);

            builder.HasIndex(c => c.ClienteId).IsUnique();

            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
