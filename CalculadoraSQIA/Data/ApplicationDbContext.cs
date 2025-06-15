using CalculadoraSQIA.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace CalculadoraSQIA.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)  { }

        public DbSet<Cotacao> Cotacoes { get; set; }
    }
}
