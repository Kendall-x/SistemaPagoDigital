using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.Models
{
    public class Cobro
    {
        public int Id { get; set; }

        public int ConductorId { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Codigo { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaExpiracion { get; set; }

        public string Estado { get; set; } = "Pendiente";

        public Usuario Conductor { get; set; } = null!;
    }
}