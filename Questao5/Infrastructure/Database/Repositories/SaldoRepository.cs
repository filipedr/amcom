using Dapper;
using System.Data;
using System.Threading.Tasks;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class SaldoRepository : ISaldoRepository
    {
        private readonly IDbConnection _dbConnection;

        public SaldoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<SaldoQueryResultado> ConsultarSaldo(Guid idcontacorrente)
        {
            var query = @"
                        SELECT 
                            cc.numero,
                            cc.nome,
                            cc.ativo,
                            SUM(CASE WHEN m.tipomovimento = 'C' THEN m.valor ELSE -m.valor END) AS Saldo 
                        FROM CONTACORRENTE cc
                        LEFT JOIN MOVIMENTO m ON m.idcontacorrente = cc.idcontacorrente
                        WHERE 
                            cc.ativo = 1 and
                            cc.idcontacorrente = @idcontacorrente
                        GROUP BY 
                            cc.numero, cc.nome";

            var resultado = await _dbConnection.QuerySingleOrDefaultAsync<SaldoQueryResultado>(query, new { idcontacorrente });

            if (resultado == null)
            {
                return new SaldoQueryResultado
                {
                    IsSuccess = false,
                    Nome = null,
                    Numero = 0,
                    Saldo = 0.00m,
                    Ativo = 0
                };
            }

            return new SaldoQueryResultado
            {
                IsSuccess = true,
                Nome = resultado.Nome,
                Numero = resultado.Numero,
                Saldo = resultado.Saldo ?? 0.00m,
                Ativo = resultado.Ativo
            };
        }
    }
}