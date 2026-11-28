using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;

namespace SistemaPagoDigital.Services
{
    public class ReporteService
    {
        private readonly AplicacionDbContext _context;

        public ReporteService(AplicacionDbContext context)
        {
            _context = context;
        }

        // Consultar los cobros de un Conductor según su estado.
        public async Task<List<Cobro>> ConsultarCobrosPorEstadoAsync(
            int conductorId,
            string estado)
        {
            return await _context.Cobros
                .Where(c => c.ConductorId == conductorId &&
                            c.Estado == estado)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        // Consultar los cobros de un Conductor dentro de un rango de fechas.
        public async Task<List<Cobro>> ConsultarCobrosPorFechaAsync(
            int conductorId,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return await _context.Cobros
                .Where(c => c.ConductorId == conductorId &&
                            c.FechaCreacion >= fechaInicio &&
                            c.FechaCreacion < fechaFin.AddDays(1))
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        // Calcular los ingresos de un Conductor dentro de un rango de fechas.
        public async Task<decimal> CalcularIngresosAsync(
            int conductorId,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return await _context.Pagos
                .Where(p => p.Cobro.ConductorId == conductorId &&
                            p.FechaPago >= fechaInicio &&
                            p.FechaPago < fechaFin.AddDays(1))
                .SumAsync(p => (decimal?)p.Monto) ?? 0;
        }
    }
}