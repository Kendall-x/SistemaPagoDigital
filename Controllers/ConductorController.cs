using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Conductor")]
    public class ConductorController : Controller
    {
        private readonly PagoService _pagoService;

        public ConductorController(PagoService pagoService)
        {
            _pagoService = pagoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Consultar los pagos recibidos por el Conductor.
        public async Task<IActionResult> PagosRecibidos()
        {
            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var pagos = await _pagoService
                .ConsultarPagosRecibidosAsync(conductorId);

            return View(pagos);
        }

        // Validar un pago recibido.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidarPago(int pagoId)
        {
            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            bool resultado = await _pagoService.ValidarPagoAsync(
                pagoId,
                conductorId
            );

            if (!resultado)
            {
                TempData["Error"] = "No se pudo validar el pago.";
            }
            else
            {
                TempData["Mensaje"] = "El pago fue validado correctamente.";
            }

            return RedirectToAction(nameof(PagosRecibidos));
        }
    }
}