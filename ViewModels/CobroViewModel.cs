using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.ViewModels
{
    public class CobroViewModel
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(150, ErrorMessage = "La descripción no puede superar los 150 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(typeof(decimal), "1", "1000", ErrorMessage = "El monto debe estar entre ₡1 y ₡1 000.")]
        public decimal Monto { get; set; }
    }
}