using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;

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
            DateTime? fechaFin)
        {
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

            ViewBag.TotalIngresos = total;

            return View();
        }
    }
}