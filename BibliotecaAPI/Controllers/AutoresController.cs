using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using BibliotecaAPI.Models.DTO;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly AppDbContext _context;
    public AutoresController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AutorDTO>>>> GetAutores()
    {
        var autoresDTO = await _context.Autores
            .AsNoTracking()
            .Select(a => new AutorDTO { AutorID = a.AutorID, Nome = a.Nome })
            .ToListAsync();

        return Ok(new ApiResponse<IEnumerable<AutorDTO>>(200, "Autores listados com sucesso", autoresDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AutorDTO>>> GetAutor(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
            return NotFound(new ApiResponse<string>(404, "Autor não encontrado"));

        var dto = new AutorDTO { AutorID = autor.AutorID, Nome = autor.Nome };
        return Ok(new ApiResponse<AutorDTO>(200, "Autor encontrado com sucesso", dto));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AutorDTO>>> CreateAutor(AutorDTO dto)
    {
        var autor = new Autor { Nome = dto.Nome };
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        dto.AutorID = autor.AutorID;
        return CreatedAtAction(nameof(GetAutor), new { id = autor.AutorID }, new ApiResponse<AutorDTO>(201, "Autor criado com sucesso", dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<AutorDTO>>> UpdateAutor(int id, AutorDTO dto)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
            return NotFound(new ApiResponse<string>(404, "Autor não encontrado"));

        autor.Nome = dto.Nome;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<AutorDTO>(200, "Autor atualizado com sucesso", dto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteAutor(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
            return NotFound(new ApiResponse<string>(404, "Autor não encontrado"));

        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<string>(200, "Autor deletado com sucesso"));
    }
}
