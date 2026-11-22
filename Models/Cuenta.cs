namespace SistemaPagoDigital.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public decimal Saldo { get; set; }

        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.Now;

        public Usuario Usuario { get; set; } = null!;
    }
}