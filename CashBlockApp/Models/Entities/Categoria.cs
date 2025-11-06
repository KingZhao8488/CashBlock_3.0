using System;
using System.Collections.Generic;

namespace CashBlockApp.Models.Entities;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> IdProductos { get; set; } = new List<Producto>();
}
