using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Models;

namespace SistemaPagoDigital.Datos
{
    public class AplicacionDbContext
        : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AplicacionDbContext(
            DbContextOptions<AplicacionDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Recarga> Recargas { get; set; }
        public DbSet<Cobro> Cobros { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Una cuenta pertenece a un usuario.
            modelBuilder.Entity<Cuenta>()
                .HasOne(c => c.Usuario)
                .WithOne()
                .HasForeignKey<Cuenta>(c => c.UsuarioId);
            modelBuilder.Entity<Cuenta>()
                .HasIndex(c => c.UsuarioId)
                .IsUnique();

            // Un conductor puede generar varios cobros.
            modelBuilder.Entity<Cobro>()
                .HasOne(c => c.Conductor)
                .WithMany()
                .HasForeignKey(c => c.ConductorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un cliente puede realizar varios pagos.
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cada pago corresponde a un cobro.
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Cobro)
                .WithMany()
                .HasForeignKey(p => p.CobroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un cobro solamente puede tener un pago.
            modelBuilder.Entity<Pago>()
                .HasIndex(p => p.CobroId)
                .IsUnique();

            // Una cuenta puede tener varias recargas.
            modelBuilder.Entity<Recarga>()
                .HasOne(r => r.Cuenta)
                .WithMany()
                .HasForeignKey(r => r.CuentaId);

            // Una cuenta puede tener varios movimientos.
            modelBuilder.Entity<Movimiento>()
                .HasOne(m => m.Cuenta)
                .WithMany()
                .HasForeignKey(m => m.CuentaId);

            // Precisión de los valores monetarios.
            modelBuilder.Entity<Cuenta>()
                .Property(c => c.Saldo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Recarga>()
                .Property(r => r.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cobro>()
                .Property(c => c.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.SaldoResultante)
                .HasPrecision(18, 2);
        }
    }
}