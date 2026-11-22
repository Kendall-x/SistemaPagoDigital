using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class CuentaController : Controller
    {
        private readonly CuentaService _cuentaService;

        public CuentaController(CuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var cuenta = await _cuentaService.ConsultarCuenta(usuarioId);

            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }
    }
}