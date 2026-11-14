namespace SistemaPagoDigital.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public decimal Saldo { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}