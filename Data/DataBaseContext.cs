using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Data.Mappings;

namespace api.Data
{
    public class DataBaseContext : DbContext
    {
       

        //Inserir Todos os Models
        public DbSet<UsuarioModel> Usuario { get; set; }
        public DbSet<PerfilModel> Perfil { get; set; }
        public DbSet<StatusModel> Status { get; set; }
        public DbSet<AtividadeModel> Atividade { get; set; }


        string connectionString = Configuration.ConnectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Inserir todos os Maps para criar as tabelas através do Migration
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PerfilMap());
            modelBuilder.ApplyConfiguration(new StatusMap());
            modelBuilder.ApplyConfiguration(new AtividadeMap());
        }


    }
}
