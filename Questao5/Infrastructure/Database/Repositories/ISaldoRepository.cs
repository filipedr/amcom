using System.Threading.Tasks;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface ISaldoRepository
    {
        Task<SaldoQueryResultado> ConsultarSaldo(Guid idcontacorrente);
    }
}