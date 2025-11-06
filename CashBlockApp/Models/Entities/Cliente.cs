using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Cliente
{
    public string CedulaCliente { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdPerfiles { get; set; }

    public virtual Perfiles? IdPerfilesNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
