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

        public async Task<Dictionary<string, decimal>> ConsultarIngresosPorPeriodoAsync(
            int conductorId,
            DateTime fechaInicio,
            DateTime fechaFin,
            string periodo)
        {
            var pagos = await _context.Pagos
                .Where(p => p.Cobro.ConductorId == conductorId &&
                            p.FechaPago >= fechaInicio &&
                            p.FechaPago < fechaFin.AddDays(1))
                .Select(p => new
                {
                    p.FechaPago,
                    p.Monto
                })
                .ToListAsync();

            var ingresos = periodo switch
            {
                "Dia" => pagos
                    .GroupBy(p => p.FechaPago.ToString("yyyy-MM-dd"))
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto)),

                "Semana" => pagos
                    .GroupBy(p =>
                    {
                        int diferencia = ((int)p.FechaPago.DayOfWeek + 6) % 7;
                        DateTime inicioSemana = p.FechaPago.Date.AddDays(-diferencia);
                        return inicioSemana.ToString("yyyy-MM-dd");
                    })
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto)),

                "Mes" => pagos
                    .GroupBy(p => p.FechaPago.ToString("yyyy-MM"))
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Monto)),

                _ => new Dictionary<string, decimal>()
            };

            return ingresos;
        }

        public async Task<Dictionary<string, int>> ConsultarCantidadCobrosPorEstadoAsync(
            int conductorId,
            DateTime fechaInicio,
            DateTime fechaFin)
                {
                    var cantidades = new Dictionary<string, int>
            {
                { "Pendiente", 0 },
                { "Pagado", 0 },
                { "Validado", 0 },
                { "Expirado", 0 },
                { "Cancelado", 0 }
            };

            var resultados = await _context.Cobros
                .Where(c => c.ConductorId == conductorId &&
                            c.FechaCreacion >= fechaInicio &&
                            c.FechaCreacion < fechaFin.AddDays(1))
                .GroupBy(c => c.Estado)
                .Select(grupo => new
                {
                    Estado = grupo.Key,
                    Cantidad = grupo.Count()
                })
                .ToListAsync();

            foreach (var resultado in resultados)
            {
                if (cantidades.ContainsKey(resultado.Estado))
                {
                    cantidades[resultado.Estado] = resultado.Cantidad;
                }
            }

            return cantidades;
        }
    }
}