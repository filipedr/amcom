using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {

        public int NumeroConta { get; private set; }
        public string Titular { get; private set; }
        private double saldo;

        public ContaBancaria(int numeroConta, string titular, double depositoInicial = 0.0)
        {
            NumeroConta = numeroConta;
            Titular = titular;
            saldo = depositoInicial;
        }

        public void Deposito(double valor)
        {
            if (valor > 0)
            {
                saldo += valor;
            }
        }

        public void Saque(double valor)
        {
            if (valor > 0)
            {
                saldo -= valor + 3.50; // Taxa de saque
            }
        }

        public void AlterarNome(string novoNome)
        {
            Titular = novoNome;
        }

        public double ObterSaldo()
        {
            return saldo;
        }

    }
}
