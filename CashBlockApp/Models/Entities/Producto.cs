using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int CantidadDisponible { get; set; }

    public int? IdProveedor { get; set; }

    public virtual ICollection<Existencia> Existencia { get; set; } = new List<Existencia>();

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual ICollection<ProductoCompra> ProductoCompras { get; set; } = new List<ProductoCompra>();

    public virtual ICollection<ProductoVenta> ProductoVenta { get; set; } = new List<ProductoVenta>();

    public virtual ICollection<Categoria> IdCategoria { get; set; } = new List<Categoria>();
}
