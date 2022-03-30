using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Mappings
{
    public class PerfilMap : IEntityTypeConfiguration<PerfilModel>
    {
        public void Configure(EntityTypeBuilder<PerfilModel> builder)
        {
            // Tabela
            builder.ToTable("tb_stc_perfil");

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



        }
    }
}
