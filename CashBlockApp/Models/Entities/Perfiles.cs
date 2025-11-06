using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Perfiles
{
    public int IdPerfiles { get; set; }

    public string Rol { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
