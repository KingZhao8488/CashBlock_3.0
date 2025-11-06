using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Almacen
{
    public int IdAlmacen { get; set; }

    public virtual ICollection<Existencia> Existencia { get; set; } = new List<Existencia>();

    public virtual ICollection<RecepcionMercancia> RecepcionMercancia { get; set; } = new List<RecepcionMercancia>();
}
