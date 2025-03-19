using ES2Backend.Models;
using System;
using System.Linq;

public class AuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Authenticate(string username, string password)
    {
        var user = _context.Utilizadores
            .FirstOrDefault(u => u.Username.Trim().ToLower() == username.Trim().ToLower());
        return user != null && user.Password.Trim() == password.Trim();
    }

    public bool UserExists(string username)
    {
        return _context.Utilizadores.Any(u => u.Username.Trim().ToLower() == username.Trim().ToLower());
    }

    public void RegisterUser(Utilizador utilizador)
    {
        utilizador.Username = utilizador.Username.Trim().ToLower();
        utilizador.Password = utilizador.Password.Trim();
        _context.Utilizadores.Add(utilizador);
        _context.SaveChanges();
    }
}
