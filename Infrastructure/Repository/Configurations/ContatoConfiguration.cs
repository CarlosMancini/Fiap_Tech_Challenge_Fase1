using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repository.Configurations
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("Contato");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("int").ValueGeneratedNever().UseIdentityColumn();
            builder.Property(p => p.ContatoNome).HasColumnType("varchar").IsRequired();
            builder.Property(p => p.ContatoTelefone).HasColumnType("varchar").IsRequired();
            builder.Property(p => p.ContatoEmail).HasColumnType("varchar").IsRequired();
            builder.Property(p => p.DddId).HasColumnType("varchar").IsRequired();

            builder.HasOne(p => p.Regiao)
            .WithMany(r => r.Contatos)
            .HasPrincipalKey(r => r.Id);
        }
    }
}
