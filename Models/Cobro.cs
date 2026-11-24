using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.Models
{
    public class Cobro
    {
        public int Id { get; set; }

        public int ConductorId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(150, ErrorMessage = "La descripción no puede superar los 150 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(typeof(decimal), "1", "1000", ErrorMessage = "El monto debe estar entre ₡1 y ₡1000.")]
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