using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;

namespace SistemaPagoDigital.Services
{
    public class RecargaService
    {
        private readonly AplicacionDbContext _context;
        public RecargaService(AplicacionDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarRecarga(int usuarioId, decimal monto)
        {
            if (monto < 1 || monto > 1000)
            {
                return false;
            }

            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cuenta == null)
            {
                return false;
            }

            using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                cuenta.Saldo += monto;
                cuenta.FechaUltimaActualizacion = DateTime.Now;

                var recarga = new Recarga
                {
                    CuentaId = cuenta.Id,
                    Monto = monto,
                    Estado = "Completada"
                };

                _context.Recargas.Add(recarga);

                var movimiento = new Movimiento
                {
                    CuentaId = cuenta.Id,
                    Tipo = "Recarga",
                    Monto = monto,
                    SaldoResultante = cuenta.Saldo,
                    Fecha = DateTime.Now
                };

                _context.Movimientos.Add(movimiento);

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return true;
            }
            catch
            {
                await transaccion.RollbackAsync();
                return false;
            }
        }
    }
}