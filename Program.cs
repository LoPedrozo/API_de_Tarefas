using System.Text.Json.Serialization;
using TAREFASAPI.Models;
using TAREFASAPI.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var prioridades = Enum.GetValues<Prioridade>();

var tarefas = Enumerable.Range(1, 100).Select(i => new Tarefa
{
    Id = i,
    Titulo = $"Tarefa {i}",
    Descricao = $"Descrição gerada automaticamente para a tarefa {i}.",
    Status = i % 3 == 0 ? "Concluída" : "Não concluída",
    Prioridade = prioridades[(i - 1) % prioridades.Length],
    Responsavel = $"Responsável {((i - 1) % 5) + 1}",
    Tags = new() { $"Tag{(i % 3) + 1}", "Inicial" },
    DataVencimento = DateTime.UtcNow.AddDays(i % 7),
    EstimativaHoras = i % 4 == 0 ? (double?)(i % 6 + 1) : null,
    DataConclusao = i % 3 == 0 ? DateTime.UtcNow.AddDays(-(i % 5)) : null
}).ToList();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok("Bem-vindo à API de Tarefas!"));

ROTA_GET.Map(app, tarefas);
ROTA_POST.Map(app, tarefas);
ROTA_DELETE.Map(app, tarefas);

app.Run();

