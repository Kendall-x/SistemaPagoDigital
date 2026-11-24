using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using SistemaPagoDigital.ViewModels;
using System.Security.Claims;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Conductor")]
    public class CobroController : Controller
    {
        private readonly CobroService _cobroService;

        public CobroController(CobroService cobroService)
        {
            _cobroService = cobroService;
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CobroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var conductorId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var cobro = await _cobroService.CrearCobroAsync(
                conductorId,
                modelo.Descripcion,
                modelo.Monto
            );

            return View("Resultado", cobro);
        }
    }
}