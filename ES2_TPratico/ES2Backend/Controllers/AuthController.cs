using Microsoft.AspNetCore.Mvc;
using ES2Backend.Models;

namespace ES2Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (_authService.Authenticate(loginRequest.Username, loginRequest.Password))
            {
                return Ok(new { message = "Login bem-sucedido!" });
            }
            return Unauthorized("Credenciais inválidas");
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            if (_authService.UserExists(registerRequest.Username))
                return BadRequest("Já existe um utilizador com esse username.");

            var newUser = new Utilizador
            {
                Nome = registerRequest.Nome,
                NumHoras = registerRequest.NumHoras,
                Username = registerRequest.Username,
                Password = registerRequest.Password
            };

            _authService.RegisterUser(newUser);
            return Ok(new { message = "Conta criada com sucesso!" });
        }
    }
}
