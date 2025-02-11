using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Questao5.Infrastructure.Database;
using Questao5.Application.Queries.Responses; 
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Database.Repositories;

namespace Questao5.Application.Queries.Handlers
{
    public class SaldoHandler : IRequestHandler<SaldoQuery, SaldoQueryResultado> 
    {
        private readonly ISaldoRepository _repository;

        public SaldoHandler(ISaldoRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaldoQueryResultado> Handle(SaldoQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ConsultarSaldo(request.IdContaCorrente); 
        }
    }
}