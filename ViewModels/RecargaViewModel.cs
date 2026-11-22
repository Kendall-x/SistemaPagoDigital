using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.ViewModels
{
    public class RecargaViewModel
    {
        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(typeof(decimal), "1", "1000",
            ErrorMessage = "El monto debe estar entre $1 y $1 000.")]
        public decimal Monto { get; set; }
    }
}
