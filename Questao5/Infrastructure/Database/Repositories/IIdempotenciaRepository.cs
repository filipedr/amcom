namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<bool> Existe(Guid chaveIdempotencia);
        Task Registrar(Guid chaveIdempotencia, string requisicao, string resultado);
        Task<string> ObterResultado(Guid chaveIdempotencia);
    }
}
