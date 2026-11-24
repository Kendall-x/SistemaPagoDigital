using Microsoft.EntityFrameworkCore;
using SistemaPagoDigital.Datos;
using SistemaPagoDigital.Models;
using System.Security.Cryptography;

namespace SistemaPagoDigital.Services
{
    public class CobroService
    {
        private readonly AplicacionDbContext _context;

        public CobroService(AplicacionDbContext context)
        {
            _context = context;
        }
        private string GenerarCodigo()
        {
            int numero = RandomNumberGenerator.GetInt32(0, 1000000);

            return numero.ToString("D6");
        }
        private async Task<string> GenerarCodigoDisponibleAsync()
        {
            string codigo;

            do
            {
                codigo = GenerarCodigo();

            } while (await _context.Cobros.AnyAsync(c =>
                c.Codigo == codigo &&
                c.Estado == "Pendiente" &&
                c.FechaExpiracion > DateTime.Now));

            return codigo;
        }

        public async Task<Cobro> CrearCobroAsync(
            int conductorId,
            string descripcion,
            decimal monto)
        {
            if (monto < 1 || monto > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(monto));
            }

            string codigo = await GenerarCodigoDisponibleAsync();

            var cobro = new Cobro
            {
                ConductorId = conductorId,
                Descripcion = descripcion,
                Monto = monto,
                Codigo = codigo,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(5),
                Estado = "Pendiente"
            };

            _context.Cobros.Add(cobro);

            await _context.SaveChangesAsync();

            return cobro;
        }
    }
}