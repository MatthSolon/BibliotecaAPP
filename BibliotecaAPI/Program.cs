using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();

    if (!db.Autores.Any())
    {
        var autor1 = new Autor { Nome = "Machado de Assis", Nacionalidade = "Brasileiro", DataNascimento = new DateTime(1839, 6, 21) };
        var autor2 = new Autor { Nome = "J.K. Rowling", Nacionalidade = "Britânica", DataNascimento = new DateTime(1965, 7, 31) };

        db.Autores.AddRange(autor1, autor2);
        db.SaveChanges();
    }

    if (!db.Generos.Any())
    {
        var genero1 = new Genero { Nome = "Romance", Descricao = "Narrativa longa com enredo estruturado." };
        var genero2 = new Genero { Nome = "Fantasia", Descricao = "Histórias com elementos mágicos." };

        db.Generos.AddRange(genero1, genero2);
        db.SaveChanges();
    }

    if (!db.Livros.Any())
    {
        var livro1 = new Livro
        {
            Titulo = "Dom Casmurro",
            AnoPublicacao = 1899,
            AutorID = db.Autores.First(a => a.Nome == "Machado de Assis").AutorID,
            GeneroID = db.Generos.First(g => g.Nome == "Romance").GeneroID
        };

        var livro2 = new Livro
        {
            Titulo = "Harry Potter e a Pedra Filosofal",
            AnoPublicacao = 1997,
            AutorID = db.Autores.First(a => a.Nome == "J.K. Rowling").AutorID,
            GeneroID = db.Generos.First(g => g.Nome == "Fantasia").GeneroID
        };

        db.Livros.AddRange(livro1, livro2);
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
