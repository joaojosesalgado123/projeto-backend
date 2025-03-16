using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ES2Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProjetosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjetosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetos()
    {
        return await _context.Projetos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Projeto>> GetProjeto(int id)
    {
        var projeto = await _context.Projetos.FindAsync(id);
        if (projeto == null)
        {
            return NotFound();
        }
        return projeto;
    }

    [HttpPost]
    public async Task<ActionResult<Projeto>> PostProjeto(Projeto projeto)
    {
        _context.Projetos.Add(projeto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProjeto), new { id = projeto.IdProjeto }, projeto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProjeto(int id, Projeto projeto)
    {
        if (id != projeto.IdProjeto)
        {
            return BadRequest();
        }
        _context.Entry(projeto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProjeto(int id)
    {
        var projeto = await _context.Projetos.FindAsync(id);
        if (projeto == null)
        {
            return NotFound();
        }
        _context.Projetos.Remove(projeto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}