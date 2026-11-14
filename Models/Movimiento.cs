namespace SistemaPagoDigital.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal SaldoResultante { get; set; }
        public DateTime Fecha { get; set; }

        public Cuenta Cuenta { get; set; } = null!;
    }
}