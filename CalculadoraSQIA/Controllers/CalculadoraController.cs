using CalculadoraSQIA.Dtos;
using CalculadoraSQIA.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculadoraSQIA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculadoraController : ControllerBase
    {
        private readonly ICalculadoraService _calculadoraService;
        private readonly ILogger<CalculadoraController> _logger;

        public CalculadoraController(ICalculadoraService calculadoraService, ILogger<CalculadoraController> logger)
        {
            _calculadoraService = calculadoraService;
            _logger = logger;
        }

        /// <summary>
        /// Calcula o fator acumulado e o valor atualizado entre duas datas.
        /// </summary>
        /// <param name="request">Dados de entrada (data inicial, data final, valor investido)</param>
        [HttpGet("calcular")]
        public async Task<IActionResult> Calcular([FromQuery] CalculoRequestDto request)
        {
            try
            {
                var resultado = await _calculadoraService.CalcularAsync(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular investimento");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Calcula dia a dia o fator acumulado e o valor atualizado para o período informado.
        /// </summary>
        /// <param name="dataInicio">Data inicial (formato: yyyy-MM-dd)</param>
        /// <param name="dataFim">Data final (formato: yyyy-MM-dd)</param>
        /// <param name="valorInvestido">Valor investido</param>
        [HttpGet("calcular-por-periodo")]
        public async Task<ActionResult<List<CalculoDiarioResponseDto>>> CalcularPorPeriodo([FromQuery] CalculoRequestDto request)
        {
            try
            {
                var resultado = await _calculadoraService.CalcularPorPeriodoAsync(request.DataAplicacao, request.DataFinal, request.ValorInvestido);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular por período.");
                return BadRequest(ex.Message);
            }
        }

    }
}
