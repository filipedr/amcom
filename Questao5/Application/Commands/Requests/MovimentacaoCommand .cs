using MediatR;
using Questao5.Application.Commands.Responses;
using System.Text.Json.Serialization;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoCommand : IRequest<Resultado>
    {
        [JsonIgnore]
        public Guid IdMovimento { get; set; }
        public string IdContaCorrente { get; set; } = string.Empty; // Recebe como string
        [JsonIgnore]
        public DateTime DataMovimento { get; set; }
        public char TipoMovimento { get; set; } // 'C' para crédito, 'D' para débito
        public decimal Valor { get; set; }
        [JsonIgnore]
        public Guid ChaveIdempotencia { get; set; }

    }
}
