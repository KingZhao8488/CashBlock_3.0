using CashBlockApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace CashBlockApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; }

        
    }
    
    
}
