using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Existencia
{
    public int IdExistencias { get; set; }

    public int? CantidadAlmacen { get; set; }

    public string? CedulaEmpleado { get; set; }

    public int? IdAlmacen { get; set; }

    public int? IdProducto { get; set; }

    public virtual Empleado? CedulaEmpleadoNavigation { get; set; }

    public virtual Almacen? IdAlmacenNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
