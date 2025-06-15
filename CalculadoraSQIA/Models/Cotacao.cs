namespace CalculadoraSQIA.Models
{
    public class Cotacao
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Indexador { get; set; }
        public decimal Valor { get; set; }

    }
}
