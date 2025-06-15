CalculadoraSQIA - Projeto de API REST com ASP.NET Core

Descrição
Este projeto é uma API REST desenvolvida em C# ASP.NET Core com foco em cálculos de rentabilidade financeira baseados no índice SQI (índice fictício para o desafio).

A API permite:

-Calcular a evolução de um valor investido em um período.
-Calcular o valor acumulado entre duas datas.
-Consultar os resultados dia a dia.

Inclui também:

Testes unitários com xUnit e Moq
Persistência de dados em SQLite

Clean Code: Services, Repositories, DTOs, Interfaces

Principais Camadas

Controllers	Endpoints da API REST

Services	Lógica de negócio / Regras de cálculo
Repositories	Acesso ao banco de dados (SQLite)
Data	Configuração do EF Core e DbContext
Models	Entidades que refletem as tabelas do banco
DTOs	Objetos de transporte de dados (Request / Response)

Test Testes unitários com xUnit + Moq

*Lembrando que o projeto inclui Logs de erros e informacoes conforme descrito no desafio para a vaga

