using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Application.Queries.Handlers;

namespace Questao5.Tests
{
    public class ContaBancariaTests
    {
        private readonly Mock<IMovimentacaoRepository> _mockMovimentacaoRepo;
        private readonly Mock<ISaldoRepository> _mockSaldoRepo;
        private readonly MovimentacaoHandler _movimentacaoHandler;
        private readonly SaldoHandler _saldoHandler;

        private readonly Mock<IIdempotenciaRepository> _mockIdempotenciaRepo;

        public ContaBancariaTests()
        {
            _mockMovimentacaoRepo = new Mock<IMovimentacaoRepository>();
            _mockIdempotenciaRepo = new Mock<IIdempotenciaRepository>(); // Adicionado

            _mockSaldoRepo = new Mock<ISaldoRepository>();
            _movimentacaoHandler = new MovimentacaoHandler(_mockMovimentacaoRepo.Object, _mockIdempotenciaRepo.Object); // Ajustado
            _saldoHandler = new SaldoHandler(_mockSaldoRepo.Object);
        }

        [Fact]
        public async Task Movimentacao_Deve_Retornar_Erro_Se_Conta_Nao_Cadastrada()
        {
            var command = new MovimentacaoCommand { IdContaCorrente = Guid.NewGuid().ToString(), TipoMovimento = 'C', Valor = 100 };
            _mockMovimentacaoRepo.Setup(repo => repo.ObterContaPorId(It.IsAny<Guid>().ToString())).ReturnsAsync((ContaCorrente)null);
            var result = await _movimentacaoHandler.Handle(command, CancellationToken.None);
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_ACCOUNT", result.ErrorMessage);
        }

        [Fact]
        public async Task Movimentacao_Deve_Retornar_Erro_Se_Conta_Inativa()
        {
            var contaId = Guid.NewGuid().ToString(); // Armazena um GUID fixo para garantir consistência
            var conta = new ContaCorrente { IdContaCorrente = contaId, Ativo = 0 };
            var command = new MovimentacaoCommand { IdContaCorrente = contaId, TipoMovimento = 'C', Valor = 100 };

          
            _mockMovimentacaoRepo.Setup(repo => repo.ObterContaPorId(contaId))
                                 .ReturnsAsync(conta);

            var result = await _movimentacaoHandler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("INACTIVE_ACCOUNT", result.ErrorMessage);
        }


        [Fact]
        public async Task Movimentacao_Deve_Retornar_Erro_Se_Valor_Invalido()
        {
            var command = new MovimentacaoCommand { IdContaCorrente = Guid.NewGuid().ToString(), TipoMovimento = 'C', Valor = 0 };
            var result = await _movimentacaoHandler.Handle(command, CancellationToken.None);
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_VALUE", result.ErrorMessage);
        }

        [Fact]
        public async Task Movimentacao_Deve_Retornar_Erro_Se_Tipo_Invalido()
        {
            var command = new MovimentacaoCommand { IdContaCorrente = Guid.NewGuid().ToString(), TipoMovimento = 'X', Valor = 100 };
            var result = await _movimentacaoHandler.Handle(command, CancellationToken.None);
            Assert.False(result.IsSuccess);
            Assert.Equal("INVALID_TYPE", result.ErrorMessage);
        }

        [Fact]
        public async Task Saldo_Deve_Retornar_Erro_Se_Conta_Inativa()
        {
            var query = new SaldoQuery { IdContaCorrente = Guid.NewGuid() };
            var resultadoEsperado = new SaldoQueryResultado { IsSuccess = false, ErrorMessage = "INACTIVE_ACCOUNT" };
            _mockSaldoRepo.Setup(repo => repo.ConsultarSaldo(It.IsAny<Guid>())).ReturnsAsync(resultadoEsperado);
            var result = await _saldoHandler.Handle(query, CancellationToken.None);
            Assert.False(result.IsSuccess);
            Assert.Equal("INACTIVE_ACCOUNT", result.ErrorMessage);
        }




    }
}