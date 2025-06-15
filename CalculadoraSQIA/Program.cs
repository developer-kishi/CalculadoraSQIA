using CalculadoraSQIA.Data;
using CalculadoraSQIA.Extensions;
using CalculadoraSQIA.Models;
using CalculadoraSQIA.Repositories;
using CalculadoraSQIA.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Criar pasta Data se não existir
var dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
if (!Directory.Exists(dataFolder))
    Directory.CreateDirectory(dataFolder);

// Configurar caminho do banco
var dbPath = Path.Combine(dataFolder, "calculadora.db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));


// injecting the repository and service
builder.Services.AddScoped<ICotacaoRepository, CotacaoRepository>();
builder.Services.AddScoped<ICalculadoraService, CalculadoraService>();
//builder.Services.AddScoped<CalculadoraService>();

var app = builder.Build();

// Criação do banco e Seed inicial
app.EnsureDatabaseCreatedAndSeeded();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseApiResponseMiddleware(); // Custom middleware for API response formatting

app.UseAuthorization();

app.MapControllers();

app.Run();
