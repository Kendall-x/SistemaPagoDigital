using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Services;
using System.Security.Claims;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class MovimientoController : Controller
    {
        private readonly MovimientoService _movimientoService;

        public MovimientoController(MovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var movimientos = await _movimientoService.ConsultarMovimientos(usuarioId);

            return View(movimientos);
        }
    }
}