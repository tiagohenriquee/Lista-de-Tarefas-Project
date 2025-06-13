using ListaDeTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefas.Data
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options)
        {
        }
        public DbSet<TabelaModel> Tarefas { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CategoriaModel>().HasData(
                new CategoriaModel{ CategoriaId = "trabalho", Nome = "Trabalho"},
                new CategoriaModel { CategoriaId = "casa", Nome = "Casa"},
                new CategoriaModel { CategoriaId = "faculdade", Nome = "Faculdade"},
                new CategoriaModel { CategoriaId = "compras", Nome = "Compras"},
                new CategoriaModel { CategoriaId = "academia", Nome = "Academia"}
                );
            modelBuilder.Entity<StatusModel>().HasData(
                new StatusModel { StatusId = "aberto", Nome = "Aberto"},
                new StatusModel { StatusId = "completo", Nome = "Completo"}
                );


            base.OnModelCreating(modelBuilder);
        }

    }
}
