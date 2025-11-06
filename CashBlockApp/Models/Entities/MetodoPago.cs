using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class MetodoPago
{
    public int IdMetodo { get; set; }

    public string TipoMetodo { get; set; } = null!;

    public string? NombreMetodo { get; set; }

    public int? IdVenta { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
