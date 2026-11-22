using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.Models
{
    public class Recarga
    {
        public int Id { get; set; }

        public int CuentaId { get; set; }

        [Required(ErrorMessage = "El monto de la recarga es obligatorio.")]
        [Range(typeof(decimal), "1", "1000",
            ErrorMessage = "El monto de la recarga debe estar entre $1 y $1 000.")]
        public decimal Monto { get; set; }

        public string Estado { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        public Cuenta Cuenta { get; set; } = null!;
    }
}