using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CashBlockApp.Models.Entities;

public partial class CashblockContext : DbContext
{
    public CashblockContext()
    {
    }

    public CashblockContext(DbContextOptions<CashblockContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Existencia> Existencias { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<OrdenCompra> OrdenCompras { get; set; }

    public virtual DbSet<Perfiles> Perfiles { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoCompra> ProductoCompras { get; set; }

    public virtual DbSet<ProductoVenta> ProductoVenta { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<RecepcionMercancia> RecepcionMercancia { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Venta { get; set; }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
  //      => optionsBuilder.UseMySql("server=localhost;port=3306;database=cashblock;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.IdAlmacen).HasName("PRIMARY");

            entity.ToTable("almacen");

            entity.Property(e => e.IdAlmacen)
                .HasColumnType("int(11)")
                .HasColumnName("id_almacen");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(100)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CedulaCliente).HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.IdPerfiles, "fk_clientes_perfiles");

            entity.HasIndex(e => e.IdUsuario, "fk_clientes_usuario");

            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(20)
                .HasColumnName("cedula_cliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdPerfiles)
                .HasColumnType("int(11)")
                .HasColumnName("id_perfiles");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdPerfilesNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdPerfiles)
                .HasConstraintName("fk_clientes_perfiles");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_clientes_usuario");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PRIMARY");

            entity.ToTable("compra");

            entity.HasIndex(e => e.CedulaEmpleado, "fk_compra_empleado");

            entity.Property(e => e.IdCompra)
                .HasColumnType("int(11)")
                .HasColumnName("id_compra");
            entity.Property(e => e.CedulaEmpleado)
                .HasMaxLength(20)
                .HasColumnName("cedula_empleado");
            entity.Property(e => e.FechaCompra).HasColumnName("fecha_compra");
            entity.Property(e => e.PrecioTotal)
                .HasPrecision(10, 2)
                .HasColumnName("precio_total");

            entity.HasOne(d => d.CedulaEmpleadoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.CedulaEmpleado)
                .HasConstraintName("fk_compra_empleado");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.CedulaEmpleado).HasName("PRIMARY");

            entity.ToTable("empleado");

            entity.HasIndex(e => e.IdPerfiles, "fk_empleado_perfiles");

            entity.HasIndex(e => e.IdUsuario, "fk_empleado_usuario");

            entity.Property(e => e.CedulaEmpleado)
                .HasMaxLength(20)
                .HasColumnName("cedula_empleado");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Cargo)
                .HasMaxLength(50)
                .HasColumnName("cargo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Egreso).HasColumnName("egreso");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdPerfiles)
                .HasColumnType("int(11)")
                .HasColumnName("id_perfiles");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Ingreso).HasColumnName("ingreso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Salario)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'1300000.00'")
                .HasColumnName("salario");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdPerfilesNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdPerfiles)
                .HasConstraintName("fk_empleado_perfiles");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_empleado_usuario");

            entity.HasMany(d => d.IdCompras).WithMany(p => p.CedulaEmpleados)
                .UsingEntity<Dictionary<string, object>>(
                    "EmpleadoCompra",
                    r => r.HasOne<Compra>().WithMany()
                        .HasForeignKey("IdCompra")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("empleado_compra_ibfk_2"),
                    l => l.HasOne<Empleado>().WithMany()
                        .HasForeignKey("CedulaEmpleado")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("empleado_compra_ibfk_1"),
                    j =>
                    {
                        j.HasKey("CedulaEmpleado", "IdCompra")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("empleado_compra");
                        j.HasIndex(new[] { "IdCompra" }, "id_compra");
                        j.IndexerProperty<string>("CedulaEmpleado")
                            .HasMaxLength(20)
                            .HasColumnName("cedula_empleado");
                        j.IndexerProperty<int>("IdCompra")
                            .HasColumnType("int(11)")
                            .HasColumnName("id_compra");
                    });
        });

        modelBuilder.Entity<Existencia>(entity =>
        {
            entity.HasKey(e => e.IdExistencias).HasName("PRIMARY");

            entity.ToTable("existencias");

            entity.HasIndex(e => e.IdAlmacen, "fk_existencias_almacen");

            entity.HasIndex(e => e.CedulaEmpleado, "fk_existencias_empleado");

            entity.HasIndex(e => e.IdProducto, "fk_existencias_producto");

            entity.Property(e => e.IdExistencias)
                .HasColumnType("int(11)")
                .HasColumnName("id_existencias");
            entity.Property(e => e.CantidadAlmacen)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_almacen");
            entity.Property(e => e.CedulaEmpleado)
                .HasMaxLength(20)
                .HasColumnName("cedula_empleado");
            entity.Property(e => e.IdAlmacen)
                .HasColumnType("int(11)")
                .HasColumnName("id_almacen");
            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_Producto");

            entity.HasOne(d => d.CedulaEmpleadoNavigation).WithMany(p => p.Existencia)
                .HasForeignKey(d => d.CedulaEmpleado)
                .HasConstraintName("fk_existencias_empleado");

            entity.HasOne(d => d.IdAlmacenNavigation).WithMany(p => p.Existencia)
                .HasForeignKey(d => d.IdAlmacen)
                .HasConstraintName("fk_existencias_almacen");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Existencia)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("fk_existencias_producto");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PRIMARY");

            entity.ToTable("factura");

            entity.HasIndex(e => e.IdVenta, "fk_factura_venta");

            entity.Property(e => e.IdFactura)
                .HasColumnType("int(11)")
                .HasColumnName("id_factura");
            entity.Property(e => e.FechaVenta).HasColumnName("Fecha_Venta");
            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.PrecioTotal)
                .HasPrecision(10, 2)
                .HasColumnName("precio_total");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("fk_factura_venta");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodo).HasName("PRIMARY");

            entity.ToTable("metodo_pago");

            entity.HasIndex(e => e.IdVenta, "fk_metodo_venta");

            entity.Property(e => e.IdMetodo)
                .HasColumnType("int(11)")
                .HasColumnName("id_Metodo");
            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.NombreMetodo)
                .HasMaxLength(100)
                .HasColumnName("Nombre_Metodo");
            entity.Property(e => e.TipoMetodo)
                .HasMaxLength(50)
                .HasColumnName("Tipo_Metodo");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.MetodoPagos)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("fk_metodo_venta");
        });

        modelBuilder.Entity<OrdenCompra>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PRIMARY");

            entity.ToTable("orden_compra");

            entity.HasIndex(e => e.IdCompra, "fk_orden_compra");

            entity.HasIndex(e => e.IdProveedor, "fk_orden_proveedor");

            entity.Property(e => e.IdOrden)
                .HasColumnType("int(11)")
                .HasColumnName("id_orden");
            entity.Property(e => e.CantidadSolicitada)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_solicitada");
            entity.Property(e => e.IdCompra)
                .HasColumnType("int(11)")
                .HasColumnName("id_compra");
            entity.Property(e => e.IdProveedor)
                .HasColumnType("int(11)")
                .HasColumnName("id_proveedor");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.OrdenCompras)
                .HasForeignKey(d => d.IdCompra)
                .HasConstraintName("fk_orden_compra");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.OrdenCompras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("fk_orden_proveedor");
        });

        modelBuilder.Entity<Perfiles>(entity =>
        {
            entity.HasKey(e => e.IdPerfiles).HasName("PRIMARY");

            entity.ToTable("perfiles");

            entity.Property(e => e.IdPerfiles)
                .HasColumnType("int(11)")
                .HasColumnName("id_perfiles");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Rol)
                .HasMaxLength(100)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdProveedor, "fk_producto_proveedor");

            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_Producto");
            entity.Property(e => e.CantidadDisponible)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_disponible");
            entity.Property(e => e.IdProveedor)
                .HasColumnType("int(11)")
                .HasColumnName("id_proveedor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioCompra)
                .HasPrecision(10, 2)
                .HasColumnName("precio_compra");
            entity.Property(e => e.PrecioVenta)
                .HasPrecision(10, 2)
                .HasColumnName("precio_venta");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("fk_producto_proveedor");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdProductos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductoCategorium",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("producto_categoria_ibfk_2"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("IdProducto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("producto_categoria_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdProducto", "IdCategoria")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("producto_categoria");
                        j.HasIndex(new[] { "IdCategoria" }, "id_categoria");
                        j.IndexerProperty<int>("IdProducto")
                            .HasColumnType("int(11)")
                            .HasColumnName("id_Producto");
                        j.IndexerProperty<int>("IdCategoria")
                            .HasColumnType("int(11)")
                            .HasColumnName("id_categoria");
                    });
        });

        modelBuilder.Entity<ProductoCompra>(entity =>
        {
            entity.HasKey(e => new { e.IdProducto, e.IdCompra })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("producto_compra");

            entity.HasIndex(e => e.IdCompra, "id_compra");

            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_Producto");
            entity.Property(e => e.IdCompra)
                .HasColumnType("int(11)")
                .HasColumnName("id_compra");
            entity.Property(e => e.CantidadComprada)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_comprada");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.ProductoCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_compra_ibfk_2");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoCompras)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_compra_ibfk_1");
        });

        modelBuilder.Entity<ProductoVenta>(entity =>
        {
            entity.HasKey(e => new { e.IdProducto, e.IdVenta })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("producto_venta");

            entity.HasIndex(e => e.IdVenta, "id_venta");

            entity.Property(e => e.IdProducto)
                .HasColumnType("int(11)")
                .HasColumnName("id_Producto");
            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.CantidadVendida)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_vendida");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_venta_ibfk_1");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.ProductoVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_venta_ibfk_2");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PRIMARY");

            entity.ToTable("proveedor");

            entity.Property(e => e.IdProveedor)
                .HasColumnType("int(11)")
                .HasColumnName("id_proveedor");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<RecepcionMercancia>(entity =>
        {
            entity.HasKey(e => e.IdRecepcion).HasName("PRIMARY");

            entity.ToTable("recepcion_mercancia");

            entity.HasIndex(e => e.IdAlmacen, "fk_recepcion_almacen");

            entity.HasIndex(e => e.IdOrden, "fk_recepcion_orden");

            entity.Property(e => e.IdRecepcion)
                .HasColumnType("int(11)")
                .HasColumnName("id_Recepcion");
            entity.Property(e => e.CantidadRecibida)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_recibida");
            entity.Property(e => e.FechaRecepcion).HasColumnName("fecha_recepcion");
            entity.Property(e => e.IdAlmacen)
                .HasColumnType("int(11)")
                .HasColumnName("id_almacen");
            entity.Property(e => e.IdOrden)
                .HasColumnType("int(11)")
                .HasColumnName("id_orden");

            entity.HasOne(d => d.IdAlmacenNavigation).WithMany(p => p.RecepcionMercancia)
                .HasForeignKey(d => d.IdAlmacen)
                .HasConstraintName("fk_recepcion_almacen");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.RecepcionMercancia)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("fk_recepcion_orden");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.IdPerfiles, "fk_usuarios_perfiles");

            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .HasColumnName("contraseña");
            entity.Property(e => e.IdPerfiles)
                .HasColumnType("int(11)")
                .HasColumnName("id_perfiles");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .HasColumnName("nombre_usuario");

            entity.HasOne(d => d.IdPerfilesNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdPerfiles)
                .HasConstraintName("fk_usuarios_perfiles");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PRIMARY");

            entity.ToTable("venta");

            entity.HasIndex(e => e.CedulaCliente, "fk_venta_cliente");

            entity.HasIndex(e => e.CedulaEmpleado, "fk_venta_empleado");

            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.CedulaCliente)
                .HasMaxLength(20)
                .HasColumnName("cedula_cliente");
            entity.Property(e => e.CedulaEmpleado)
                .HasMaxLength(20)
                .HasColumnName("cedula_empleado");
            entity.Property(e => e.FechaVenta).HasColumnName("fecha_venta");
            entity.Property(e => e.PrecioTotal)
                .HasPrecision(10, 2)
                .HasColumnName("precio_total");
            entity.Property(e => e.TotalProductos)
                .HasColumnType("int(11)")
                .HasColumnName("total_productos");

            entity.HasOne(d => d.CedulaClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.CedulaCliente)
                .HasConstraintName("fk_venta_cliente");

            entity.HasOne(d => d.CedulaEmpleadoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.CedulaEmpleado)
                .HasConstraintName("fk_venta_empleado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
