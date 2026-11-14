namespace SistemaPagoDigital.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int CobroId { get; set; }
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }

        public Cobro Cobro { get; set; } = null!;
        public Usuario Cliente { get; set; } = null!;
    }
}