using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Database.Repositories;

public class MovimentacaoHandler : IRequestHandler<MovimentacaoCommand, Resultado>
{
    private readonly IMovimentacaoRepository _movimentacaoRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;

    public MovimentacaoHandler(IMovimentacaoRepository movimentacaoRepository, IIdempotenciaRepository idempotenciaRepository)
    {
        _movimentacaoRepository = movimentacaoRepository;
        _idempotenciaRepository = idempotenciaRepository;
    }

    public async Task<Resultado> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
    {

        // Verificar se a chave de idempotência já foi processada
        if (await _idempotenciaRepository.Existe(request.ChaveIdempotencia))
        {
            // Retornar o resultado armazenado
            var resultado = await _idempotenciaRepository.ObterResultado(request.ChaveIdempotencia);
            return new Resultado { IsSuccess = true, Message = resultado }; 
        }

        if (request.Valor <= 0)
            return new Resultado { 
                IsSuccess = false, 
                ErrorMessage = "INVALID_VALUE", 
                Message = "O valor da movimentação deve ser maior que zero." 
            };

        if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
            return new Resultado { 
                IsSuccess = false, 
                ErrorMessage = "INVALID_TYPE",
                Message = "O tipo de movimentação deve ser 'C' para crédito ou 'D' para débito."
            };

        var conta = await _movimentacaoRepository.ObterContaPorId(request.IdContaCorrente);
        if (conta == null)
            return new Resultado { 
                IsSuccess = false, 
                ErrorMessage = "INVALID_ACCOUNT",
                Message = "Conta corrente não encontrada."
            };

        if (conta.Ativo == 0)
            return new Resultado { 
                IsSuccess = false, 
                ErrorMessage = "INACTIVE_ACCOUNT",
                Message = "A conta corrente está inativa."
            };

        var movimentoResultado = await _movimentacaoRepository.RegistrarMovimentacao(request);

        // Registrar o resultado na tabela de idempotência
        await _idempotenciaRepository.Registrar(movimentoResultado.ChaveIdempotencia, movimentoResultado.Requisicao, movimentoResultado.Message);

        return movimentoResultado;
    }
}