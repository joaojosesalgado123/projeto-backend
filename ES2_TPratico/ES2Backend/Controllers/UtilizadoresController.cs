using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ES2Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UtilizadoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UtilizadoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadores()
    {
        return await _context.Utilizadores.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Utilizador>> GetUtilizador(int id)
    {
        var utilizador = await _context.Utilizadores.FindAsync(id);
        if (utilizador == null)
        {
            return NotFound();
        }
        return utilizador;
    }

    [HttpPost]
    public async Task<ActionResult<Utilizador>> PostUtilizador(Utilizador utilizador)
    {
        _context.Utilizadores.Add(utilizador);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.IdUtilizador }, utilizador);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUtilizador(int id, Utilizador utilizador)
    {
        if (id != utilizador.IdUtilizador)
        {
            return BadRequest();
        }

        _context.Entry(utilizador).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UtilizadorExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUtilizador(int id)
    {
        var utilizador = await _context.Utilizadores
            .Include(u => u.Membros)
            .Include(u => u.Projetos)
            .Include(u => u.IdTarefas)
            .FirstOrDefaultAsync(u => u.IdUtilizador == id);

        if (utilizador == null)
        {
            return NotFound();
        }

        // Remove Membros associados
        _context.Membros.RemoveRange(utilizador.Membros);

        // Remove Projetos associados
        _context.Projetos.RemoveRange(utilizador.Projetos);

        // Remove relações na TarefaUtilizador
        var tarefasAssociadas = await _context.Tarefas
            .Where(t => t.IdUtilizadors.Any(u => u.IdUtilizador == id))
            .ToListAsync();

        foreach (var tarefa in tarefasAssociadas)
        {
            tarefa.IdUtilizadors.Remove(utilizador);
        }

        // Remove Utilizador
        _context.Utilizadores.Remove(utilizador);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UtilizadorExists(int id)
    {
        return _context.Utilizadores.Any(e => e.IdUtilizador == id);
    }
}
