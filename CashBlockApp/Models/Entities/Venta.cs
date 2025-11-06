using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? TotalProductos { get; set; }

    public DateOnly? FechaVenta { get; set; }

    public decimal? PrecioTotal { get; set; }

    public string? CedulaEmpleado { get; set; }

    public string? CedulaCliente { get; set; }

    public virtual Cliente? CedulaClienteNavigation { get; set; }

    public virtual Empleado? CedulaEmpleadoNavigation { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<MetodoPago> MetodoPagos { get; set; } = new List<MetodoPago>();

    public virtual ICollection<ProductoVenta> ProductoVenta { get; set; } = new List<ProductoVenta>();
}
