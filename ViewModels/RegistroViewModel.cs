using System.ComponentModel.DataAnnotations;

namespace SistemaPagoDigital.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(80)]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20)]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;
    }
}
