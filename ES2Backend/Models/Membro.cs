using System;
using System.Collections.Generic;

namespace ES2Backend.Models;

public partial class Membro
{
    public int IdMembro { get; set; }

    public string Nome { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Estado { get; set; }

    public int IdUtilizador { get; set; }

    public int IdProjeto { get; set; }

    public virtual Projeto IdProjetoNavigation { get; set; } = null!;

    public virtual Utilizador IdUtilizadorNavigation { get; set; } = null!;
}
