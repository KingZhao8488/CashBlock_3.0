using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Factura
{
    public int IdFactura { get; set; }

    public decimal? PrecioTotal { get; set; }

    public DateOnly? FechaVenta { get; set; }

    public int? IdVenta { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
