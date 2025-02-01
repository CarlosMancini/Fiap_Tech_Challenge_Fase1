using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Repository.Configurations
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("Contato");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("int").ValueGeneratedOnAdd();
            builder.Property(p => p.ContatoNome).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(p => p.ContatoTelefone).HasColumnType("varchar(11)").HasMaxLength(11).IsRequired();
            builder.Property(p => p.ContatoEmail).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(p => p.RegiaoId).HasColumnType("int").IsRequired();

            builder.HasOne(p => p.Regiao)
            .WithMany(r => r.Contatos)
            .HasPrincipalKey(r => r.Id);
        }
    }
}
