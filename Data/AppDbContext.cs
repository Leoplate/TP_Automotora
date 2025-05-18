using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        public DbSet<Posventa> Posventas { get; set; }

        public DbSet<Tipo> Tipo { get; set; }

        public DbSet<Estado> Estado { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        // Configuración de Cliente
        //    modelBuilder.Entity<Cliente>(entity =>
        //    {
        //        entity.ToTable("Clientes");
        //        entity.HasKey(c => c.Id); // Clave primaria
        //        entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
        //        entity.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
        //        entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
        //        entity.Property(c => c.Telefono).HasMaxLength(20);
        //    });

        // Configuración de Producto
        //    modelBuilder.Entity<Producto>(entity =>
        //    {
        //        entity.ToTable("Productos");
        //        entity.HasKey(p => p.Id); // Clave primaria
        //        entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        //        entity.Property(p => p.Precio).HasColumnType("decimal(10,2)").IsRequired();
        //        entity.Property(p => p.Stock).IsRequired();
        //    });

        // Configuración de Venta y relaciones
        //    modelBuilder.Entity<Venta>(entity =>
        //    {
        //        entity.ToTable("Ventas"); // Nombre de la tabla en la base de datos
        //        entity.HasKey(v => v.Id); // Configuración de la clave primaria

        // Relación con Cliente
        //        entity.HasOne(v => v.Cliente)
        //            .WithMany() // Cliente tiene muchas Ventas
        //            .HasForeignKey(v => v.ClienteId) // Clave foránea
        //            .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada opcional

        // Relación con Producto (Vehículo)
        //        entity.HasOne(v => v.Vehiculo)
        //            .WithMany() // Producto tiene muchas Ventas
        //            .HasForeignKey(v => v.VehiculoId) // Clave foránea
        //            .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada opcional
        //    });
        //}
    }
}



//public static void Seed(ModelBuilder modelBuilder)
//{
//    modelBuilder.Entity<Producto>().HasData(
//    new Producto { Id = 1, Nombre = "Peugeot 208", Precio = 100m, Stock = 10 },
//    new Producto { Id = 2, Nombre = "Citroen C4", Precio = 200m, Stock = 20 },
//    new Producto { Id = 3, Nombre = "BMW A4", Precio = 300m, Stock = 30 },
//    new Producto { Id = 4, Nombre = "Ford Focus", Precio = 400m, Stock = 40 },
//    new Producto { Id = 5, Nombre = "Renault Cangoo", Precio = 500m, Stock = 50 },
//    new Producto { Id = 6, Nombre = "Peugeot 2008", Precio = 600m, Stock = 60 },
//    new Producto { Id = 7, Nombre = "Citroen Psara", Precio = 700m, Stock = 70 },
//    new Producto { Id = 8, Nombre = "Ford Ka", Precio = 800m, Stock = 80 },
//    new Producto { Id = 9, Nombre = "Voslkwagen Polo", Precio = 900m, Stock = 90 },
//    new Producto { Id = 10, Nombre = "Volskwagen Track", Precio = 1000m, Stock = 100 }
//    );
//}
