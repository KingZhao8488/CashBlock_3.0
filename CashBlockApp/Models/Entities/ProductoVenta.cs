using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class ProductoVenta
{
    public int IdProducto { get; set; }

    public int IdVenta { get; set; }

    public int? CantidadVendida { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
