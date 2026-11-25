using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using SistemaPagoDigital.ViewModels;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class PagoController : Controller
    {
        private readonly PagoService _pagoService;

        public PagoController(PagoService pagoService)
        {
            _pagoService = pagoService;
        }

        // Mostrar el formulario para realizar un pago.
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Procesar el pago del Cliente.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PagoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            int clienteId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            bool resultado = await _pagoService.RealizarPagoAsync(
                clienteId,
                modelo.Codigo
            );

            if (!resultado)
            {
                ModelState.AddModelError(
                    "Codigo",
                    "No se pudo realizar el pago. Verifique el código, su vigencia y su saldo."
                );

                return View(modelo);
            }

            TempData["Mensaje"] = "¡Pago realizado correctamente!";

            return RedirectToAction("Index", "Cuenta");
        }
    }
}