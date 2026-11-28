using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;
using SistemaPagoDigital.ViewModels;

namespace SistemaPagoDigital.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        private readonly AplicacionDbContext _context;

        public AccountController(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
             AplicacionDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            if (modelo.Rol != "Cliente" && modelo.Rol != "Conductor")
            {
                ModelState.AddModelError("Rol", "Seleccione un rol válido.");
                return View(modelo);
            }

            if (!await _roleManager.RoleExistsAsync(modelo.Rol))
            {
                ModelState.AddModelError("Rol", "El rol seleccionado no es válido.");
                return View(modelo);
            }

            using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                // Crear el objeto Usuario
                var usuario = new Usuario
                {
                    UserName = modelo.Correo,
                    Email = modelo.Correo,
                    Nombre = modelo.Nombre,
                    Apellidos = modelo.Apellidos,
                    Identificacion = modelo.Identificacion,
                    FechaRegistro = DateTime.Now
                };

                // Crear el usuario en Identity
                var resultado = await _userManager.CreateAsync(usuario, modelo.Contrasena);

                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(modelo);
                }

                // Asignar el rol
                var resultadoRol = await _userManager.AddToRoleAsync(usuario, modelo.Rol);

                if (!resultadoRol.Succeeded)
                {
                    foreach (var error in resultadoRol.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(modelo);
                }

                // Crear cuenta para el usuario (Ya sea Cliente o Conductor)
                var cuenta = new Cuenta
                {
                    UsuarioId = usuario.Id,
                    Saldo = 0
                };

                _context.Cuentas.Add(cuenta);
                await _context.SaveChangesAsync();

                // Confirmar la transacción
                await transaccion.CommitAsync();

                TempData["Mensaje"] = "¡Usuario creado correctamente!";

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();

                ModelState.AddModelError(
                    string.Empty,
                    "Ocurrió un error al registrar el usuario."
                );

                return View(modelo);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await _signInManager.PasswordSignInAsync(
                modelo.Correo,
                modelo.Contrasena,
                isPersistent: false,
                lockoutOnFailure: true
            );

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(
                string.Empty,
                "Correo o contraseña incorrectos."
            );

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
