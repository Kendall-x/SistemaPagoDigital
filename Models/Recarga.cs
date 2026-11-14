namespace SistemaPagoDigital.Models
{
    public class Recarga
    {
        public int Id { get; set; }

        public int CuentaId { get; set; }

        public decimal Monto { get; set; }

        public string Estado { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public Cuenta Cuenta { get; set; } = null!;
    }
}