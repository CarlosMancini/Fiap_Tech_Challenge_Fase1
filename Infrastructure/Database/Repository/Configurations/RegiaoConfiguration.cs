using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Repository.Configurations
{
    public class RegiaoConfiguration : IEntityTypeConfiguration<Regiao>
    {
        public void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.ToTable("Regiao");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("int").ValueGeneratedNever().UseIdentityColumn();
            builder.Property(p => p.RegiaoNome).HasColumnType("varchar").IsRequired();
            builder.Property(p => p.RegiaoDdd).HasColumnType("int").IsRequired();
        }
    }
}
