using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace api.Data.Mappings
{
    public class AtividadeMap : IEntityTypeConfiguration<AtividadeModel>
    {
        public void Configure(EntityTypeBuilder<AtividadeModel> builder)
        {

            // Tabela
            builder.ToTable("tb_cad_atividade");

            // Chave Primária
            builder.HasKey(x => x.codigo);

            // Identity
            builder.Property(x => x.codigo)
            .IsRequired()
            .HasColumnName("codigo")
           .HasColumnType("BIGINT");

            builder.Property(x => x.numero_documento)
           .IsRequired()
           .HasColumnName("numero_documento")
           .HasColumnType("BIGINT");

            builder.Property(x => x.titulo)
           .IsRequired()
           .HasColumnName("titulo")
           .HasColumnType("VARCHAR")
           .HasMaxLength(150);

            builder.Property(x => x.descricao)
           .IsRequired()
           .HasColumnName("descricao")
           .HasColumnType("VARCHAR")
           .HasMaxLength(500);

            builder.Property(x => x.data_criacao)
           .IsRequired()
           .HasColumnName("data_criacao")
           .HasColumnType("DATETIME")
           .HasDefaultValue(DateTime.Now);

            builder.Property(x => x.data_alteracao)
            .HasColumnName("data_alteracao")
            .HasColumnType("DATETIME")
            .HasDefaultValue(null);

            builder.Property(x => x.solicitante)
           .HasColumnName("solicitante")
           .HasColumnType("VARCHAR")
           .HasMaxLength(150)
           .HasDefaultValue(null);

            builder.Property(x => x.codigo_status)
           .IsRequired()
           .HasColumnName("codigo_status")
           .HasColumnType("int");

           builder.Property(x => x.data_planejamento)
           .HasColumnName("data_planejamento")
           .HasColumnType("DATETIME")
           .HasDefaultValue(null);

           builder.Property(x => x.codigo_criador)
           .IsRequired()
           .HasColumnName("codigo_criador")
           .HasColumnType("BIGINT");

           //Relaciinamento
            builder
                .HasOne(x => x.status)
                .WithMany(x => x.atividade)
                .HasConstraintName("FK_Atividade_Status")
                .HasForeignKey(y => y.codigo_status);

            //Relaciinamento
            builder
                .HasOne(x => x.criador)
                .WithMany(x => x.criador)
                .HasConstraintName("FK_Atividade_Criador")
                .HasForeignKey(y => y.codigo_criador);

        
            
        }
    }
}
