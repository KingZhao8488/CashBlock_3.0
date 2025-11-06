using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class ProductoCompra
{
    public int IdProducto { get; set; }

    public int IdCompra { get; set; }

    public int? CantidadComprada { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
