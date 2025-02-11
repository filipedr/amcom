using FluentAssertions.Common;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registrar repositórios
builder.Services.AddTransient<ISaldoRepository, SaldoRepository>();
builder.Services.AddTransient<IMovimentacaoRepository, MovimentacaoRepository>();
builder.Services.AddTransient<IIdempotenciaRepository, IdempotenciaRepository>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(MovimentacaoHandler).Assembly);

// sqlite
// Registrar a conexão com o banco de dados SQLite
builder.Services.AddTransient<IDbConnection>(sp =>
    new SqliteConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<DatabaseConfig>(sp =>
{
    return new DatabaseConfig
    {
        Name = builder.Configuration.GetConnectionString("DefaultConnection")
    };
});

builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Adiciona serviços do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Banco",
        Version = "v1",
        Description = "Documentação da API para operações bancárias",
        Contact = new OpenApiContact
        {
            Name = "Filipe Dutra Rodrigues",
            Email = "filipedr@gmail.com"
        }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Banco v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


