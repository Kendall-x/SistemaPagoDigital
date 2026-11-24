using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;

namespace SistemaPagoDigital.Services
{
    public class MovimientoService
    {
        private readonly AplicacionDbContext _context;

        public MovimientoService(AplicacionDbContext context)
        {
            _context = context;
        }
        public async Task<List<Movimiento>> ConsultarMovimientos(int usuarioId)
        {
            return await _context.Movimientos
                .Where(m => m.Cuenta.UsuarioId == usuarioId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }
    }
}