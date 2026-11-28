using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;
using System.Text;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Conductor")]
    public class ReporteController : Controller
    {
        private readonly ReporteService _reporteService;

        public ReporteController(ReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        // Consultar cobros según su estado.
        public async Task<IActionResult> CobrosPorEstado(string estado = "Pendiente")
        {
            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var cobros = await _reporteService
                .ConsultarCobrosPorEstadoAsync(conductorId, estado);

            ViewBag.EstadoSeleccionado = estado;

            return View(cobros);
        }

        // Exportar cobros por estado a CSV.
        public async Task<IActionResult> ExportarCobrosPorEstado(string estado = "Pendiente")
        {
            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var cobros = await _reporteService
                .ConsultarCobrosPorEstadoAsync(conductorId, estado);

            var contenido = new StringBuilder();

            contenido.AppendLine("Descripcion;Monto;Codigo;Fecha de creacion;Estado");

            foreach (var cobro in cobros)
            {
                contenido.AppendLine(
                    $"\"{cobro.Descripcion.Replace("\"", "\"\"")}\";" +
                    $"{cobro.Monto};" +
                    $"{cobro.Codigo};" +
                    $"{cobro.FechaCreacion:yyyy-MM-dd HH:mm:ss};" +
                    $"{cobro.Estado}"
                );
            }

            byte[] archivo = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(contenido.ToString()))
                .ToArray();

            return File(
                archivo,
                "text/csv",
                $"Cobros_{estado}.csv"
            );
        }

        // Consultar cobros dentro de un rango de fechas.
        public async Task<IActionResult> CobrosPorFecha(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                return View(new List<SistemaPagoDigital.Models.Cobro>());
            }

            if (fechaInicio.Value.Date > fechaFin.Value.Date)
            {
                ModelState.AddModelError("",
                    "La fecha inicial no puede ser posterior a la fecha final.");

                return View(new List<SistemaPagoDigital.Models.Cobro>());
            }

            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var cobros = await _reporteService.ConsultarCobrosPorFechaAsync(
                conductorId,
                fechaInicio.Value.Date,
                fechaFin.Value.Date
            );

            return View(cobros);
        }

        // Consultar ingresos dentro de un rango de fechas.
        public async Task<IActionResult> Ingresos(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string periodo = "Dia")
        {
            if (periodo != "Dia" &&
                periodo != "Semana" &&
                periodo != "Mes")
            {
                periodo = "Dia";
            }

            ViewBag.PeriodoSeleccionado = periodo;

            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                ViewBag.TotalIngresos = 0;
                return View();
            }

            if (fechaInicio.Value.Date > fechaFin.Value.Date)
            {
                ModelState.AddModelError("",
                    "La fecha inicial no puede ser posterior a la fecha final.");

                ViewBag.TotalIngresos = 0;
                return View();
            }

            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            decimal total = await _reporteService.CalcularIngresosAsync(
                conductorId,
                fechaInicio.Value.Date,
                fechaFin.Value.Date
            );

            var ingresosPorPeriodo =
                await _reporteService.ConsultarIngresosPorPeriodoAsync(
                    conductorId,
                    fechaInicio.Value.Date,
                    fechaFin.Value.Date,
                    periodo
                );

            ViewBag.TotalIngresos = total;
            ViewBag.IngresosPorPeriodo = ingresosPorPeriodo;
            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");

            return View();
        }

        public async Task<IActionResult> CantidadCobrosPorEstado(
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                return View();
            }

            if (fechaInicio.Value.Date > fechaFin.Value.Date)
            {
                ModelState.AddModelError(
                    "",
                    "La fecha inicial no puede ser posterior a la fecha final.");

                return View();
            }

            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var cantidades = await _reporteService
                .ConsultarCantidadCobrosPorEstadoAsync(
                    conductorId,
                    fechaInicio.Value.Date,
                    fechaFin.Value.Date);

            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");

            return View(cantidades);
        }
    }
}