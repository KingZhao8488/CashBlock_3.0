
namespace CashBlockApp.ViewModels
{
    public class ReporteUsuarioViewModel
    {
        // Datos básicos del usuario
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }

        // Para Clientes (Pedidos/Compras)
        public int TotalPedidos { get; set; }
        public string Productos { get; set; }
        public decimal MontoTotal { get; set; }

        // Para Empleados (Ventas)
        public int TotalVentas { get; set; }
        public string ProductosVendidos { get; set; }
        public decimal MontoVendido { get; set; }
    }
}