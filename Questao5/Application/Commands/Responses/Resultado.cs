namespace Questao5.Application.Commands.Responses
{
    public class Resultado
    {
        public bool IsSuccess { get; set; }
        public Guid Id { get; set; }
        public Guid ChaveIdempotencia { get; set; }
        public string? Requisicao { get; set; }
        public string? ErrorMessage { get; set; } 
        public string? Message { get; set; }
        
    }
}
