using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using BibliotecaAPI.Models.DTO;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly AppDbContext _context;

    public LivrosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/livro
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LivroDTO>>>> GetLivros()
    {
        var livrosDTO = await _context.Livros
            .AsNoTracking()
            .Select(l => new LivroDTO
            {
                LivroID = l.LivroID,
                Titulo = l.Titulo,
                AnoPublicacao = l.AnoPublicacao,
                Autor = new AutorDTO
                {
                    AutorID = l.Autor.AutorID,
                    Nome = l.Autor.Nome
                },
                Genero = new GeneroDTO
                {
                    GeneroID = l.Genero.GeneroID,
                    Nome = l.Genero.Nome
                }
            })
            .ToListAsync();

        return Ok(new ApiResponse<IEnumerable<LivroDTO>>(200, "Livros listados com sucesso", livrosDTO));
    }

    // GET: api/livro/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> GetLivro(int id)
    {
        var livro = await _context.Livros
            .AsNoTracking()
            .Where(l => l.LivroID == id)
            .Select(l => new LivroDTO
            {
                LivroID = l.LivroID,
                Titulo = l.Titulo,
                AnoPublicacao = l.AnoPublicacao,
                Autor = new AutorDTO
                {
                    AutorID = l.Autor.AutorID,
                    Nome = l.Autor.Nome
                },
                Genero = new GeneroDTO
                {
                    GeneroID = l.Genero.GeneroID,
                    Nome = l.Genero.Nome
                }
            })
            .FirstOrDefaultAsync();

        if (livro == null)
            return NotFound(new ApiResponse<string>(404, "Livro não encontrado"));

        return Ok(new ApiResponse<LivroDTO>(200, "Livro encontrado com sucesso", livro));
    }

    // POST: api/livro
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> CreateLivro(LivroDTO dto)
    {
        var autor = await _context.Autores.FindAsync(dto.Autor.AutorID);
        var genero = await _context.Generos.FindAsync(dto.Genero.GeneroID);

        if (autor == null)
            return NotFound(new ApiResponse<string>(404, "Autor não encontrado"));

        if (genero == null)
            return NotFound(new ApiResponse<string>(404, "Gênero não encontrado"));

        var livro = new Livro
        {
            Titulo = dto.Titulo,
            AnoPublicacao = dto.AnoPublicacao,
            AutorID = autor.AutorID,
            GeneroID = genero.GeneroID
        };

        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        dto.LivroID = livro.LivroID;
        dto.Autor = new AutorDTO { AutorID = autor.AutorID, Nome = autor.Nome };
        dto.Genero = new GeneroDTO { GeneroID = genero.GeneroID, Nome = genero.Nome };

        return CreatedAtAction(nameof(GetLivro), new { id = livro.LivroID }, new ApiResponse<LivroDTO>(201, "Livro criado com sucesso", dto));
    }

    // PUT: api/livro/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> UpdateLivro(int id, LivroDTO dto)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro == null)
            return NotFound(new ApiResponse<string>(404, "Livro não encontrado"));

        var autor = await _context.Autores.FindAsync(dto.Autor.AutorID);
        var genero = await _context.Generos.FindAsync(dto.Genero.GeneroID);

        if (autor == null)
            return NotFound(new ApiResponse<string>(404, "Autor não encontrado"));

        if (genero == null)
            return NotFound(new ApiResponse<string>(404, "Gênero não encontrado"));

        livro.Titulo = dto.Titulo;
        livro.AnoPublicacao = dto.AnoPublicacao;
        livro.AutorID = autor.AutorID;
        livro.GeneroID = genero.GeneroID;

        await _context.SaveChangesAsync();

        dto.LivroID = livro.LivroID;
        dto.Autor = new AutorDTO { AutorID = autor.AutorID, Nome = autor.Nome };
        dto.Genero = new GeneroDTO { GeneroID = genero.GeneroID, Nome = genero.Nome };

        return Ok(new ApiResponse<LivroDTO>(200, "Livro atualizado com sucesso", dto));
    }

    // DELETE: api/livro/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteLivro(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro == null)
            return NotFound(new ApiResponse<string>(404, "Livro não encontrado"));

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<string>(200, "Livro deletado com sucesso"));
    }
}
