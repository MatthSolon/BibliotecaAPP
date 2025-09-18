using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using BibliotecaAPI.Models.DTO;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class GenerosController : ControllerBase
{
    private readonly AppDbContext _context;
    public GenerosController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<GeneroDTO>>>> GetGeneros()
    {
        var generosDTO = await _context.Generos
            .AsNoTracking()
            .Select(g => new GeneroDTO { GeneroID = g.GeneroID, Nome = g.Nome })
            .ToListAsync();

        return Ok(new ApiResponse<IEnumerable<GeneroDTO>>(200, "Gêneros listados com sucesso", generosDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GeneroDTO>>> GetGenero(int id)
    {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null)
            return NotFound(new ApiResponse<string>(404, "Gênero não encontrado"));

        var dto = new GeneroDTO { GeneroID = genero.GeneroID, Nome = genero.Nome };
        return Ok(new ApiResponse<GeneroDTO>(200, "Gênero encontrado com sucesso", dto));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<GeneroDTO>>> CreateGenero(GeneroDTO dto)
    {
        var genero = new Genero { Nome = dto.Nome };
        _context.Generos.Add(genero);
        await _context.SaveChangesAsync();

        dto.GeneroID = genero.GeneroID;
        return CreatedAtAction(nameof(GetGenero), new { id = genero.GeneroID }, new ApiResponse<GeneroDTO>(201, "Gênero criado com sucesso", dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<GeneroDTO>>> UpdateGenero(int id, GeneroDTO dto)
    {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null)
            return NotFound(new ApiResponse<string>(404, "Gênero não encontrado"));

        genero.Nome = dto.Nome;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<GeneroDTO>(200, "Gênero atualizado com sucesso", dto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteGenero(int id)
    {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null)
            return NotFound(new ApiResponse<string>(404, "Gênero não encontrado"));

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<string>(200, "Gênero deletado com sucesso"));
    }
}
