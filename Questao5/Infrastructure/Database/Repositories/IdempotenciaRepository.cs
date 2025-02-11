using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IDbConnection _dbConnection;

        public IdempotenciaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> Existe(Guid chaveIdempotencia)
        {
            string query = "SELECT COUNT(1) FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var count = await _dbConnection.ExecuteScalarAsync<int>(query, new { ChaveIdempotencia = chaveIdempotencia });
            return count > 0;
        }

        public async Task Registrar(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            string query = "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";
            await _dbConnection.ExecuteAsync(query, new { ChaveIdempotencia = chaveIdempotencia, Requisicao = requisicao, Resultado = resultado });
        }

        public async Task<string> ObterResultado(Guid chaveIdempotencia)
        {
            string query = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            return await _dbConnection.QuerySingleOrDefaultAsync<string>(query, new { ChaveIdempotencia = chaveIdempotencia });
        }
    }
}
