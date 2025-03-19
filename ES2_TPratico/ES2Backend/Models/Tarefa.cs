using System;
using System.Collections.Generic;

namespace ES2Backend.Models;

public partial class Tarefa
{
    public int IdTarefa { get; set; }

    public string Descricao { get; set; } = null!;

    public DateOnly DataInicio { get; set; }

    public decimal PrecoHora { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Projeto> IdProjetos { get; set; } = new List<Projeto>();

    public virtual ICollection<Utilizador> IdUtilizadors { get; set; } = new List<Utilizador>();
}
