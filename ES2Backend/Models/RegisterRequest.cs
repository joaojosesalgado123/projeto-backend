namespace ES2Backend.Models
{
    public class RegisterRequest
    {
        public string Nome { get; set; } = string.Empty;
        public decimal NumHoras { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
