using CalculadoraSQIA.Models;

namespace CalculadoraSQIA.Repositories
{
    public interface ICotacaoRepository
    {
        Task<Cotacao> ObterCotacaoPorDataAsync(DateTime data);

        Task<List<Cotacao>> ObterCotacaoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    }
}
