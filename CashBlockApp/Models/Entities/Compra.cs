using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Compra
{
    public int IdCompra { get; set; }

    public decimal? PrecioTotal { get; set; }

    public DateOnly? FechaCompra { get; set; }

    public string? CedulaEmpleado { get; set; }

    public virtual Empleado? CedulaEmpleadoNavigation { get; set; }

    public virtual ICollection<OrdenCompra> OrdenCompras { get; set; } = new List<OrdenCompra>();

    public virtual ICollection<ProductoCompra> ProductoCompras { get; set; } = new List<ProductoCompra>();

    public virtual ICollection<Empleado> CedulaEmpleados { get; set; } = new List<Empleado>();
}
