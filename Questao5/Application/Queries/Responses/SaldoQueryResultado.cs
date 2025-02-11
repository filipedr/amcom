namespace Questao5.Application.Queries.Responses
{
    public class SaldoQueryResultado
    {
        public bool IsSuccess { get; set; }
        public int? Numero { get; set; }
        public string? Nome { get; set; }
        public decimal? Saldo { get; set; }
        public int? Ativo { get; set; }
        public string? ErrorMessage { get; set; } // Para armazenar mensagens de erro, se necessário
    }
}
