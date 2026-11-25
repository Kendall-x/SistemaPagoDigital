using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.ViewModels
{
    public class PagoViewModel
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(6, MinimumLength = 6,
            ErrorMessage = "El código debe tener exactamente 6 dígitos.")]
        [RegularExpression(@"^\d{6}$",
            ErrorMessage = "El código solo debe contener números.")]
        public string Codigo { get; set; } = string.Empty;
    }
}