using CalculadoraSQIA.Data;
using CalculadoraSQIA.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraSQIA.Extensions
{
    public static class DatabaseExtensions
    {
        public static void EnsureDatabaseCreatedAndSeeded(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();

                if (!db.Cotacoes.Any())
                {
                    db.Cotacoes.AddRange(new List<Cotacao>
                    {
                        new Cotacao { Id = 1, Data = DateTime.Parse("2025-01-01"), Indexador = "SQI", Valor = 10.50m },
                        new Cotacao { Id = 2, Data = DateTime.Parse("2025-01-02"), Indexador = "SQI", Valor = 10.50m },
                        new Cotacao { Id = 3, Data = DateTime.Parse("2025-01-03"), Indexador = "SQI", Valor = 10.50m },
                        new Cotacao { Id = 6, Data = DateTime.Parse("2025-01-06"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 7, Data = DateTime.Parse("2025-01-07"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 8, Data = DateTime.Parse("2025-01-08"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 9, Data = DateTime.Parse("2025-01-09"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 10, Data = DateTime.Parse("2025-01-10"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 13, Data = DateTime.Parse("2025-01-13"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 14, Data = DateTime.Parse("2025-01-14"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 15, Data = DateTime.Parse("2025-01-15"), Indexador = "SQI", Valor = 12.25m },
                        new Cotacao { Id = 16, Data = DateTime.Parse("2025-01-16"), Indexador = "SQI", Valor = 9.00m },
                        new Cotacao { Id = 17, Data = DateTime.Parse("2025-01-17"), Indexador = "SQI", Valor = 9.00m },
                        new Cotacao { Id = 20, Data = DateTime.Parse("2025-01-20"), Indexador = "SQI", Valor = 9.00m },
                        new Cotacao { Id = 21, Data = DateTime.Parse("2025-01-21"), Indexador = "SQI", Valor = 7.75m },
                        new Cotacao { Id = 22, Data = DateTime.Parse("2025-01-22"), Indexador = "SQI", Valor = 7.75m },
                        new Cotacao { Id = 23, Data = DateTime.Parse("2025-01-23"), Indexador = "SQI", Valor = 7.75m },
                        new Cotacao { Id = 24, Data = DateTime.Parse("2025-01-24"), Indexador = "SQI", Valor = 7.75m },
                        new Cotacao { Id = 27, Data = DateTime.Parse("2025-01-27"), Indexador = "SQI", Valor = 8.25m },
                        new Cotacao { Id = 28, Data = DateTime.Parse("2025-01-28"), Indexador = "SQI", Valor = 8.25m },
                        new Cotacao { Id = 29, Data = DateTime.Parse("2025-01-29"), Indexador = "SQI", Valor = 8.25m },
                        new Cotacao { Id = 30, Data = DateTime.Parse("2025-01-30"), Indexador = "SQI", Valor = 8.25m },
                        new Cotacao { Id = 31, Data = DateTime.Parse("2025-01-31"), Indexador = "SQI", Valor = 8.25m },
                    });

                    db.SaveChanges();
                }
            }
        }
    }
}
