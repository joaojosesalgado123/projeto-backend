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

    // GET: api/utilizadores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizadores()
    {
        return await _context.Utilizadores.ToListAsync();
    }

    // GET: api/utilizadores/{id}
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

    // POST: api/utilizadores
    [HttpPost]
    public async Task<ActionResult<Utilizador>> PostUtilizador(Utilizador utilizador)
    {
        _context.Utilizadores.Add(utilizador);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.IdUtilizador }, utilizador);
    }

    // PUT: api/utilizadores/{id}
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

    // DELETE: api/utilizadores/{id}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUtilizador(int id)
{
    var utilizador = await _context.Utilizadores
        .Include(u => u.Membros)  // Inclui os membros relacionados
        .Include(u => u.Projetos) // Inclui os projetos relacionados
        .Include(u => u.IdTarefas) // Inclui as tarefas associadas
        .FirstOrDefaultAsync(u => u.IdUtilizador == id);

    if (utilizador == null)
    {
        return NotFound();
    }

    // Remove os membros associados
    _context.Membros.RemoveRange(utilizador.Membros);

    // Remove os projetos associados
    _context.Projetos.RemoveRange(utilizador.Projetos);

    // Remove associações na tabela de junção TarefaUtilizador
    var tarefasAssociadas = await _context.Tarefas
        .Where(t => t.IdUtilizadors.Any(u => u.IdUtilizador == id))
        .ToListAsync();

    foreach (var tarefa in tarefasAssociadas)
    {
        tarefa.IdUtilizadors.Remove(utilizador);
    }

    // Remove o utilizador
    _context.Utilizadores.Remove(utilizador);

    await _context.SaveChangesAsync();

    return NoContent();
}
    private bool UtilizadorExists(int id)
    {
        return _context.Utilizadores.Any(e => e.IdUtilizador == id);
    }
}