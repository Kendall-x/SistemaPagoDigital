using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaPagoDigital.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}