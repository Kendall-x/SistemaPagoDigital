using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;
using SistemaPagoDigital.ViewModels;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class RecargaController : Controller
    {
        private readonly RecargaService _recargaService;

        public RecargaController(RecargaService recargaService)
        {
            _recargaService = recargaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RecargaViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", modelo);
            }

            var usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var resultado = await _recargaService.RegistrarRecarga(
                usuarioId, modelo.Monto
            );

            if (!resultado)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No fue posible realizar la recarga."
                );

                return View("Index", modelo);
            }

            return RedirectToAction("Index", "Cuenta");
        }
    }
}