using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<UsuarioModel>
    {
        public void Configure(EntityTypeBuilder<UsuarioModel> builder)
        {
            // Tabela
            builder.ToTable("tb_cad_usuario");

            // Chave Primária
            builder.HasKey(x => x.codigo);
            
            // Identity
            builder.Property(x => x.codigo)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();  

            builder.Property(x => x.usuario)
                .IsRequired()
                .HasColumnName("usuario")
                .HasColumnType("VARCHAR")
                .HasMaxLength(150);

            builder.Property(x => x.senha)
                .IsRequired()
                .HasColumnName("senha")
                .HasColumnType("VARCHAR")
                .HasMaxLength(150);

            builder.Property(x => x.nome)
                .IsRequired()
                .HasColumnName("nome")
                .HasColumnType("VARCHAR")
                .HasMaxLength(150);

            builder.Property(x => x.codigo_perfil)
            .IsRequired()
            .HasColumnName("codigo_perfil")
            .HasColumnType("INT");

            builder.Property(x => x.ativo)
              .IsRequired()
              .HasColumnName("ativo")
              .HasColumnType("BIT");

            // Índices
            builder
                .HasIndex(x => x.usuario, "IX_usuario")
                .IsUnique();


            //Relacionamento
            builder
                .HasOne(x => x.perfil)                            //Tenho 1 perfil 
                .WithMany(x => x.usuario)                         //Esse Perfil relaciona apenas com 1 Usuário - relação 1 x 1 Para relacionamento NxN deve ser colocado como iList<> no model
                .HasConstraintName("FK_Perfil_Usuario")           //Insere nome do relacionamento
                .HasForeignKey(y => y.codigo_perfil);


          
        }


    }
}