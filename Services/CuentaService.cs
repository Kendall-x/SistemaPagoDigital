using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;

namespace SistemaPagoDigital.Services
{
    public class CuentaService
    {
        private readonly AplicacionDbContext _context;

        public CuentaService(AplicacionDbContext context)
        {
            _context = context;
        }
        public async Task<Cuenta?> ConsultarCuenta(int usuarioId)
        {
            return await _context.Cuentas
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }
    }
}
