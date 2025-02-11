using Xunit;

namespace Questao1.Tests
{
    public class ContaBancariaTests
    {
        [Fact]
        public void Deposito_DeveAumentarSaldo()
        {
            var conta = new ContaBancaria(5447, "Milton Gonçalves", 350.00);
            conta.Deposito(200);
            Assert.Equal(550.00, conta.ObterSaldo());
        }

        [Fact]
        public void Saque_DeveDiminuirSaldo_ComTaxa()
        {
            var conta = new ContaBancaria(5447, "Milton Gonçalves", 550.00);
            conta.Saque(199);
            Assert.Equal(347.50, conta.ObterSaldo()); // 550.00 - 199 - 3.50
        }

        [Fact]
        public void AlterarNome_DeveAtualizarTitular()
        {
            var conta = new ContaBancaria(5447, "Milton Gonçalves", 347.50);
            conta.AlterarNome("Milton Gonçalves da Silva");
            Assert.Equal("Milton Gonçalves da Silva", conta.Titular);
        }
    }
}