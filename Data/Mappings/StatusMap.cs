using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Mappings
{
    public class StatusMap : IEntityTypeConfiguration<StatusModel>
    {
        public void Configure(EntityTypeBuilder<StatusModel> builder)
        {
            // Tabela
            builder.ToTable("tb_stc_status");

            // Chave Primária
            builder.HasKey(x => x.codigo);

            // Identity
            builder.Property(x => x.codigo)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();

            builder.Property(x => x.descricao)
                .IsRequired()
                .HasColumnName("descricao")
                .HasColumnType("VARCHAR")
                .HasMaxLength(150);

            builder.Property(x => x.ativo)
               .IsRequired()
               .HasColumnName("ativo")
               .HasColumnType("BIT");

        }
    }
}
