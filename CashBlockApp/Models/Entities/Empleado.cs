using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Empleado
{
    public string CedulaEmpleado { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public decimal? Salario { get; set; }

    public string Cargo { get; set; } = null!;

    public DateOnly? Ingreso { get; set; }

    public DateOnly? Egreso { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdPerfiles { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Existencia> Existencia { get; set; } = new List<Existencia>();

    public virtual Perfiles? IdPerfilesNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual ICollection<Compra> IdCompras { get; set; } = new List<Compra>();
}
