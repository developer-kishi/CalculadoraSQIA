using CalculadoraSQIA.Dtos;
using CalculadoraSQIA.Repositories;
using System.Runtime.CompilerServices;

namespace CalculadoraSQIA.Services
{
    public class CalculadoraService : ICalculadoraService
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly ILogger<CalculadoraService> _logger;
        public CalculadoraService(ICotacaoRepository cotacaoRepository, ILogger<CalculadoraService> logger)
        {
            _cotacaoRepository = cotacaoRepository;
            _logger = logger;
        }
        public async Task<CalculoResponseDto> CalcularAsync(CalculoRequestDto request)
        {
            if (request.DataFinal <= request.DataAplicacao)
            {
                _logger.LogError("Data final deve ser posterior à data de aplicação.");
                throw new ArgumentException("Data final deve ser posterior à data de aplicação.");
            }
            decimal fatorAcmulado = 1m;
            DateTime dataAtual = request.DataAplicacao.AddDays(1); // Começa no dia seguinte à data de aplicação

            while (dataAtual < request.DataFinal)
            {
                if (DiaUtil(dataAtual))
                {
                    DateTime dataReferencia = ObterDataReferencia(dataAtual); // sincrono
                    var cotacao = await _cotacaoRepository.ObterCotacaoPorDataAsync(dataReferencia);
                    if (cotacao == null)
                    {
                        _logger.LogWarning($"Cotação não encontrada para a data {dataReferencia.ToShortDateString()}.");
                       // throw new InvalidOperationException($"Cotação não encontrada para a data {dataReferencia.ToShortDateString()}.");

                        dataAtual = dataAtual.AddDays(1);
                        continue;
                    }
                    decimal taxaAnual = cotacao.Valor;
                    double fatorDiario = Math.Pow((double)(1 + taxaAnual / 100), 1.0 / 252.0); // 252 dias úteis no ano
                    fatorAcmulado *= (decimal)fatorDiario;
                    fatorAcmulado = Truncar(fatorAcmulado, 16); // Truncar para 16 casas decimais conforme documentacao
                }
                dataAtual = dataAtual.AddDays(1);

            }
            decimal valorAtualizado = Truncar(request.ValorInvestido * fatorAcmulado, 8); // conforme documentacao

            return new CalculoResponseDto
            {
                FatorAcmulado = fatorAcmulado,
                ValorAtualizado = valorAtualizado
            };
        }

        public async Task<List<CalculoDiarioResponseDto>> CalcularPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, decimal valorInvestido)
        {
            if (dataInicio > dataFim)
            {
                _logger.LogError("Data inicial não pode ser maior que a data final.");
                throw new ArgumentException("Data inicial não pode ser maior que a data final.");
            }

            var cotacoes = await _cotacaoRepository.ObterCotacaoPorPeriodoAsync(dataInicio, dataFim);

            if (cotacoes == null || !cotacoes.Any())
            {
                _logger.LogWarning("Nenhuma cotação encontrada para o período informado.");
                throw new InvalidOperationException("Nenhuma cotação encontrada no período informado.");
            }

            var resultados = new List<CalculoDiarioResponseDto>();
            decimal fatorAcumulado = 1m;

            foreach (var data in cotacoes.OrderBy(c => c.Data))
            {
                if (DiaUtil(data.Data))
                {
                    decimal taxaAnual = data.Valor;
                    double fatorDiario = Math.Pow((double)(1 + taxaAnual / 100), 1.0 / 252.0);
                    fatorAcumulado *= (decimal)fatorDiario;
                    fatorAcumulado = Truncar(fatorAcumulado, 16);

                    decimal valorAtualizado = Truncar(valorInvestido * fatorAcumulado, 8);

                    resultados.Add(new CalculoDiarioResponseDto
                    {
                        Data = data.Data,
                        FatorAcumulado = fatorAcumulado,
                        ValorAtualizado = valorAtualizado
                    });
                }
            }

            return resultados;
        }


        private bool DiaUtil(DateTime data) => data.DayOfWeek != DayOfWeek.Saturday && data.DayOfWeek != DayOfWeek.Sunday;

        private DateTime ObterDataReferencia(DateTime data)
        {
           DateTime dataReferencia = data.AddDays(-1);
           while (!DiaUtil(dataReferencia))
           {
                dataReferencia = dataReferencia.AddDays(-1);
           }                
           return dataReferencia;                       
        }

        private decimal Truncar(decimal valor, int casasDecimais)
        {
            decimal fator = (decimal)Math.Pow(10, casasDecimais); // a base 10 é importante para truncar corretamente em calculos financeiros e operacoes com casads decimais 
            return Math.Truncate(valor * fator) / fator;
        }

    }
}
    

