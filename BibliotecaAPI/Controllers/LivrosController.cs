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
    public LivrosController(AppDbContext context) => _context = context;

    // GET: api/Livros
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LivroDTO>>>> GetLivros()
    {
        var livros = await _context.Livros
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .AsNoTracking()
            .Select(l => new LivroDTO
            {
                LivroID = l.LivroID,
                Titulo = l.Titulo,
                AnoPublicacao = l.AnoPublicacao,
                AutorID = l.AutorID,
                NomeAutor = l.Autor.Nome,
                GeneroID = l.GeneroID,
                NomeGenero = l.Genero.Nome
            })
            .ToListAsync();

        return Ok(new ApiResponse<IEnumerable<LivroDTO>>(200, "Livros listados com sucesso", livros));
    }

    // GET: api/Livros/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> GetLivro(int id)
    {
        var livro = await _context.Livros
            .Include(l => l.Autor)
            .Include(l => l.Genero)
            .FirstOrDefaultAsync(l => l.LivroID == id);

        if (livro == null)
            return NotFound(new ApiResponse<string>(404, "Livro não encontrado"));

        var dto = new LivroDTO
        {
            LivroID = livro.LivroID,
            Titulo = livro.Titulo,
            AnoPublicacao = livro.AnoPublicacao,
            AutorID = livro.AutorID,
            NomeAutor = livro.Autor.Nome,
            GeneroID = livro.GeneroID,
            NomeGenero = livro.Genero.Nome
        };

        return Ok(new ApiResponse<LivroDTO>(200, "Livro encontrado com sucesso", dto));
    }

    // POST: api/Livros
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> CreateLivro([FromBody] LivroDTO dto)
    {
        var autor = await _context.Autores.FindAsync(dto.AutorID);
        var genero = await _context.Generos.FindAsync(dto.GeneroID);

        if (autor == null)
            return NotFound(new ApiResponse<string>(404, "Autor não encontrado"));
        if (genero == null)
            return NotFound(new ApiResponse<string>(404, "Gênero não encontrado"));

        var livro = new Livro
        {
            Titulo = dto.Titulo,
            AnoPublicacao = dto.AnoPublicacao,
            AutorID = dto.AutorID,
            GeneroID = dto.GeneroID
        };

        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        dto.LivroID = livro.LivroID;
        dto.NomeAutor = autor.Nome;
        dto.NomeGenero = genero.Nome;

        return CreatedAtAction(nameof(GetLivro), new { id = livro.LivroID },
            new ApiResponse<LivroDTO>(201, "Livro criado com sucesso", dto));
    }

    // PUT: api/Livros/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<LivroDTO>>> UpdateLivro(int id, [FromBody] LivroDTO dto)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro == null)
            return NotFound(new ApiResponse<string>(404, "Livro não encontrado"));

        livro.Titulo = dto.Titulo;
        livro.AnoPublicacao = dto.AnoPublicacao;
        livro.AutorID = dto.AutorID;
        livro.GeneroID = dto.GeneroID;

        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<LivroDTO>(200, "Livro atualizado com sucesso", dto));
    }

    // DELETE: api/Livros/5
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
