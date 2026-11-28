using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;
using System.Text;

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

        // Exportar los pagos recibidos a CSV.
        public async Task<IActionResult> ExportarPagosRecibidos()
        {
            int conductorId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var pagos = await _pagoService
                .ConsultarPagosRecibidosAsync(conductorId);

            var contenido = new StringBuilder();

            contenido.AppendLine(
                "Fecha de pago;Codigo;Cliente;Monto;Estado de validacion"
            );

            foreach (var pago in pagos)
            {
                contenido.AppendLine(
                    $"{pago.FechaPago:yyyy-MM-dd HH:mm:ss};" +
                    $"{pago.Cobro.Codigo};" +
                    $"\"{pago.Cliente.Nombre} {pago.Cliente.Apellidos}\";" +
                    $"{pago.Monto};" +
                    $"{pago.Cobro.Estado}"
                );
            }

            byte[] archivo = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(contenido.ToString()))
                .ToArray();

            return File(
                archivo,
                "text/csv",
                "Pagos_Recibidos.csv"
            );
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