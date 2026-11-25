using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace SistemaPagoDigital.Services
{
    public class PagoService
    {
        private readonly AplicacionDbContext _context;

        public PagoService(AplicacionDbContext context)
        {
            _context = context;
        }
        public async Task<Cobro?> BuscarCobroPorCodigoAsync(string codigo)
        {
            var cobro = await _context.Cobros
                .FirstOrDefaultAsync(c => c.Codigo == codigo);

            if (cobro == null)
            {
                return null;
            }

            if (cobro.Estado != "Pendiente")
            {
                return null;
            }

            if (cobro.FechaExpiracion <= DateTime.Now)
            {
                cobro.Estado = "Expirado";
                await _context.SaveChangesAsync();

                return null;
            }

            return cobro;
        }
        public async Task<bool> TieneSaldoSuficienteAsync(
            int clienteId,
            decimal monto)
        {
            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(c => c.UsuarioId == clienteId);

            if (cuenta == null)
            {
                return false;
            }

            return cuenta.Saldo >= monto;
        }
        public async Task<bool> RealizarPagoAsync(int clienteId, string codigo)
        {
            await using var transaccion =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Buscar el cobro mediante el código.
                var cobro = await _context.Cobros
                    .FirstOrDefaultAsync(c => c.Codigo == codigo);

                // Comprobar que el cobro existe y está pendiente.
                if (cobro == null || cobro.Estado != "Pendiente")
                {
                    return false;
                }

                // Comprobar si el código ha vencido.
                if (cobro.FechaExpiracion <= DateTime.Now)
                {
                    cobro.Estado = "Expirado";

                    await _context.SaveChangesAsync();
                    await transaccion.CommitAsync();

                    return false;
                }

                // Buscar la cuenta del Cliente.
                var cuentaCliente = await _context.Cuentas
                    .FirstOrDefaultAsync(c => c.UsuarioId == clienteId);

                // Comprobar que el Cliente tenga una cuenta.
                if (cuentaCliente == null)
                {
                    return false;
                }

                // Comprobar que tenga saldo suficiente.
                if (cuentaCliente.Saldo < cobro.Monto)
                {
                    return false;
                }

                // Buscar la cuenta interna del Conductor.
                var cuentaConductor = await _context.Cuentas
                    .FirstOrDefaultAsync(c => c.UsuarioId == cobro.ConductorId);

                // Comprobar que el Conductor tenga una cuenta.
                if (cuentaConductor == null)
                {
                    return false;
                }

                // Descontar el monto de la cuenta del Cliente.
                cuentaCliente.Saldo -= cobro.Monto;
                cuentaCliente.FechaUltimaActualizacion = DateTime.Now;

                // Aumentar el saldo de la cuenta del Conductor.
                cuentaConductor.Saldo += cobro.Monto;
                cuentaConductor.FechaUltimaActualizacion = DateTime.Now;

                // Cambiar el estado del cobro.
                cobro.Estado = "Pagado";

                // Registrar el pago realizado.
                var pago = new Pago
                {
                    CobroId = cobro.Id,
                    ClienteId = clienteId,
                    Monto = cobro.Monto,
                    FechaPago = DateTime.Now
                };

                _context.Pagos.Add(pago);

                // Registrar el movimiento del Cliente.
                var movimientoCliente = new Movimiento
                {
                    CuentaId = cuentaCliente.Id,
                    Tipo = "Pago realizado",
                    Monto = cobro.Monto,
                    SaldoResultante = cuentaCliente.Saldo,
                    Fecha = DateTime.Now,
                    Estado = "Completado"
                };

                _context.Movimientos.Add(movimientoCliente);

                // Registrar el movimiento del Conductor.
                var movimientoConductor = new Movimiento
                {
                    CuentaId = cuentaConductor.Id,
                    Tipo = "Cobro recibido",
                    Monto = cobro.Monto,
                    SaldoResultante = cuentaConductor.Saldo,
                    Fecha = DateTime.Now,
                    Estado = "Completado"
                };

                _context.Movimientos.Add(movimientoConductor);

                // Guardar todos los cambios en la base de datos.
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

        // Consultar los pagos recibidos por un Conductor.
        public async Task<List<Pago>> ConsultarPagosRecibidosAsync(int conductorId)
        {
            return await _context.Pagos
                .Include(p => p.Cobro)
                .Include(p => p.Cliente)
                .Where(p => p.Cobro.ConductorId == conductorId)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();
        }

        // Validar un pago recibido por el Conductor.
        public async Task<bool> ValidarPagoAsync(int pagoId, int conductorId)
        {
            var pago = await _context.Pagos
                .Include(p => p.Cobro)
                .FirstOrDefaultAsync(p =>
                    p.Id == pagoId &&
                    p.Cobro.ConductorId == conductorId);

            if (pago == null || pago.Cobro.Estado != "Pagado")
            {
                return false;
            }

            pago.Cobro.Estado = "Validado";

            await _context.SaveChangesAsync();

            return true;
        }

    }
}