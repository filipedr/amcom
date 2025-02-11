using Dapper;
using System.Data;
using System.Threading.Tasks;
using Questao5.Application.Commands;
using Questao5.Application.Queries;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly IDbConnection _dbConnection;

        public MovimentacaoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Resultado> RegistrarMovimentacao(MovimentacaoCommand command)
        {
            var query = "INSERT INTO MOVIMENTO (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor); SELECT last_insert_rowid();";

            //Gera um novo ID para IdMovimento
            Guid idMovimento = Guid.NewGuid();
            //Data
            DateTime dataMovimento = DateTime.Now;
            //
            Guid iDempotencia = Guid.NewGuid();

            // Adiciona os parametros
            var parameters = new MovimentacaoCommand
            {
                IdMovimento = idMovimento,
                IdContaCorrente = command.IdContaCorrente,
                DataMovimento = dataMovimento,
                TipoMovimento = command.TipoMovimento,
                Valor = command.Valor
            };

            var id = await _dbConnection.ExecuteScalarAsync<int>(query, parameters);

            var requisicao = $"IdMovimento: {idMovimento}, " +
                  $"IdContaCorrente: {command.IdContaCorrente}, " +
                  $"DataMovimento: {dataMovimento}, " +
                  $"TipoMovimento: {command.TipoMovimento}, " +
                  $"Valor: {command.Valor}";

            return new Resultado { 
                IsSuccess = true, 
                Id = idMovimento,
                ChaveIdempotencia = iDempotencia,
                Requisicao = requisicao,
                Message = "Movimentação da conta feita com sucesso!"
            };
        }

        public async Task<ContaCorrente> ObterContaPorId(string contaId)
        {
            string query = "SELECT idcontacorrente, numero, nome, ativo FROM CONTACORRENTE WHERE idcontacorrente = @Id";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { Id = contaId });

            if (result != null)
            {
                return new ContaCorrente
                {
                    IdContaCorrente = result.IdContaCorrente, 
                    Numero = result.Numero,
                    Nome = result.Nome,
                    Ativo = result.Ativo
                };
            }
            return null; 
        }
    }
    
}