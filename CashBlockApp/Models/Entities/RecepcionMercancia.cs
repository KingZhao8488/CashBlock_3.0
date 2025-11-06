using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class RecepcionMercancia
{
    public int IdRecepcion { get; set; }

    public int? CantidadRecibida { get; set; }

    public DateOnly? FechaRecepcion { get; set; }

    public int? IdOrden { get; set; }

    public int? IdAlmacen { get; set; }

    public virtual Almacen? IdAlmacenNavigation { get; set; }

    public virtual OrdenCompra? IdOrdenNavigation { get; set; }
}
