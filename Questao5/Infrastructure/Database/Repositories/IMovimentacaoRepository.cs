using System.Threading.Tasks;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IMovimentacaoRepository
    {
        Task<Resultado> RegistrarMovimentacao(MovimentacaoCommand command);
        Task<ContaCorrente> ObterContaPorId(string contaId);
    }

}
