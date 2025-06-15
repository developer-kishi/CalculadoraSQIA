using Xunit;
using Moq;
using CalculadoraSQIA.Models;
using CalculadoraSQIA.Services;
using CalculadoraSQIA.Dtos;
using CalculadoraSQIA.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CalculadoraSQIA.Tests.Services
{
    public class CalculadoraServiceTests
    {
        [Fact]
        public async Task CalcularAsync_DeveCalcularCorretamente_FatorEValor()
        {
            // Arrange
            var mockRepo = new Mock<ICotacaoRepository>();
            var mockLogger = new Mock<ILogger<CalculadoraService>>();

            var dataAplicacao = new DateTime(2025, 1, 13);
            var dataFinal = new DateTime(2025, 1, 25);
            decimal valorInvestido = 10000m;

            // Simulando cotações do banco para o período
            var cotacoes = new List<Cotacao>
            {
                new Cotacao { Data = new DateTime(2025, 1, 13), Valor = 12.25m },
                new Cotacao { Data = new DateTime(2025, 1, 14), Valor = 12.25m },
                new Cotacao { Data = new DateTime(2025, 1, 15), Valor = 12.25m },
                new Cotacao { Data = new DateTime(2025, 1, 16), Valor = 9.00m },
                new Cotacao { Data = new DateTime(2025, 1, 17), Valor = 9.00m },
                new Cotacao { Data = new DateTime(2025, 1, 20), Valor = 9.00m },
                new Cotacao { Data = new DateTime(2025, 1, 21), Valor = 7.75m },
                new Cotacao { Data = new DateTime(2025, 1, 22), Valor = 7.75m },
                new Cotacao { Data = new DateTime(2025, 1, 23), Valor = 7.75m },
                new Cotacao { Data = new DateTime(2025, 1, 24), Valor = 7.75m },
            };

            mockRepo.Setup(r => r.ObterCotacaoPorPeriodoAsync(dataAplicacao, dataFinal))
                    .ReturnsAsync(cotacoes);

            var service = new CalculadoraService(mockRepo.Object, mockLogger.Object);

            var request = new CalculoRequestDto
            {
                ValorInvestido = valorInvestido,
                DataAplicacao = dataAplicacao,
                DataFinal = dataFinal
            };

            // Act
            var resultado = await service.CalcularAsync(request);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.FatorAcmulado > 0);
            Assert.True(resultado.ValorAtualizado > 0);

            // Resultado esperado calculado manualmente:
            double fatorEsperado = 1.003295651851955;
            decimal valorEsperado = 10000m * (decimal)fatorEsperado;

            Assert.InRange((double)resultado.FatorAcmulado, fatorEsperado - 0.00001, fatorEsperado + 0.00001);
            Assert.InRange((double)resultado.ValorAtualizado, (double)(valorEsperado - 0.01m), (double)(valorEsperado + 0.01m));
        }
    }
}
