using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class OrdenCompra
{
    public int IdOrden { get; set; }

    public int? CantidadSolicitada { get; set; }

    public int? IdCompra { get; set; }

    public int? IdProveedor { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual ICollection<RecepcionMercancia> RecepcionMercancia { get; set; } = new List<RecepcionMercancia>();
}
