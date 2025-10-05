using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Autor>()
                .HasMany(a => a.Livros)
                .WithOne(l => l.Autor)
                .HasForeignKey(l => l.AutorID);

            modelBuilder.Entity<Genero>()
                .HasMany(g => g.Livros)
                .WithOne(l => l.Genero)
                .HasForeignKey(l => l.GeneroID);
        }
    }
}