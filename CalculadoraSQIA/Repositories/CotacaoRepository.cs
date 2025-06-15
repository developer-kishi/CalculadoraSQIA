using CalculadoraSQIA.Models;
using CalculadoraSQIA.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraSQIA.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly ApplicationDbContext _context;
        public CotacaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Cotacao> ObterCotacaoPorDataAsync(DateTime data)
        {
            return await _context.Cotacoes
                .Where(c => c.Data.Date == data.Date).FirstOrDefaultAsync();

            //.FirstOrDefaultAsync(c => c.Data.Date == data.Date);
        }
        public async Task<List<Cotacao>> ObterCotacaoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Cotacoes
                .Where(c => c.Data.Date >= dataInicio.Date && c.Data.Date <= dataFim.Date)
                .OrderBy(c => c.Data)
                .ToListAsync();
        }
    }
    
}
