using CalculadoraSQIA.Dtos;
using CalculadoraSQIA.Models;
using CalculadoraSQIA.Repositories;
using CalculadoraSQIA.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CalculadoraSQIA.Tests.Services
{
    public class CalculadoraServiceTests
    {
        private readonly Mock<ICotacaoRepository> _cotacaoRepositoryMock;
        private readonly Mock<ILogger<CalculadoraService>> _loggerMock;
        private readonly ICalculadoraService _calculadoraService;

        public CalculadoraServiceTests()
        {
            _cotacaoRepositoryMock = new Mock<ICotacaoRepository>();
            _loggerMock = new Mock<ILogger<CalculadoraService>>();

            _calculadoraService = new CalculadoraService(_cotacaoRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CalcularAsync_DeveRetornarCalculo()
        {
            // Arrange
            var request = new CalculoRequestDto
            {
                ValorInvestido = 1000m,
                DataAplicacao = new DateTime(2025, 1, 1),
                DataFinal = new DateTime(2025, 1, 3)
            };

            var cotacaoAplicacao = new Cotacao
            {
                Data = request.DataAplicacao,
                Valor = 10.5m
            };

            var cotacaoFinal = new Cotacao
            {
                Data = request.DataFinal,
                Valor = 12.25m
            };

            // Mock para buscar cotação na data de aplicação
            _cotacaoRepositoryMock
                .Setup(r => r.ObterCotacaoPorDataAsync(request.DataAplicacao))
                .ReturnsAsync(cotacaoAplicacao);

            // Mock para buscar cotação na data final
            _cotacaoRepositoryMock
                .Setup(r => r.ObterCotacaoPorDataAsync(request.DataFinal))
                .ReturnsAsync(cotacaoFinal);

            // Act
            var resultado = await _calculadoraService.CalcularAsync(request);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.FatorAcmulado > 0);
            Assert.True(resultado.ValorAtualizado > 0);
        }

        [Fact]
        public async Task CalcularPorPeriodoAsync_DeveRetornarCalculoDiario()
        {
            // Arrange
            var dataInicio = new DateTime(2025, 1, 1);
            var dataFim = new DateTime(2025, 1, 3);
            var valorInvestido = 1000m;

            var cotacoes = new List<Cotacao>
            {
                new Cotacao { Data = new DateTime(2025, 1, 1), Valor = 10.5m },
                new Cotacao { Data = new DateTime(2025, 1, 2), Valor = 10.5m },
                new Cotacao { Data = new DateTime(2025, 1, 3), Valor = 10.5m }
            };

            _cotacaoRepositoryMock
                .Setup(r => r.ObterCotacaoPorPeriodoAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync((DateTime inicio, DateTime fim) =>
                    cotacoes.Where(c => c.Data >= inicio && c.Data <= fim).ToList()
                );

            // Act
            var resultado = await _calculadoraService.CalcularPorPeriodoAsync(dataInicio, dataFim, valorInvestido);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count);

            foreach (var item in resultado)
            {
                Assert.True(item.FatorAcumulado > 1);
                Assert.True(item.ValorAtualizado > valorInvestido);
            }
        }
    }
}
