using CalculadoraSQIA.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculadoraSQIA.Services
{
    public interface ICalculadoraService
    {
        Task<CalculoResponseDto> CalcularAsync(CalculoRequestDto request);
        Task<List<CalculoDiarioResponseDto>> CalcularPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, decimal valorInvestido);
    }
}
